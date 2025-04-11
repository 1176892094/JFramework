using System;
using System.Linq;
using UnityEngine;

namespace Astraia.Net
{
    public class NetworkAnimator : NetworkBehaviour
    {
        public Animator animator;

        private AnimatorControllerParameter[] animatorParams;

        private int[] animationHash;

        private int[] transitionHash;

        private float[] layerWeight;

        private int[] lastIntParams;

        private bool[] lastBoolParams;

        private float[] lastFloatParams;

        private double sendTime;

        private void Awake()
        {
            animatorParams = animator.parameters.Where(parameter => !animator.IsParameterControlledByCurve(parameter.nameHash)).ToArray();
            lastIntParams = new int[animatorParams.Length];
            lastBoolParams = new bool[animatorParams.Length];
            lastFloatParams = new float[animatorParams.Length];
            animationHash = new int[animator.layerCount];
            transitionHash = new int[animator.layerCount];
            layerWeight = new float[animator.layerCount];
        }

        private void Update()
        {
            if (!animator.enabled) return;
            if (isServer && isVerify)
            {
                if (sendTime < Time.unscaledTimeAsDouble + syncInterval)
                {
                    sendTime = Time.unscaledTimeAsDouble + syncInterval;
                    using var setter = MemorySetter.Pop();
                    if (WriteParameter(setter))
                    {
                        ParameterClientRpc(setter);
                    }
                }

                for (var layer = 0; layer < animator.layerCount; layer++)
                {
                    if (IsModify(layer, out var stateHash, out var stateTime))
                    {
                        using var setter = MemorySetter.Pop();
                        WriteParameter(setter);
                        AnimationClientRpc(stateHash, stateTime, layer, layerWeight[layer], setter);
                    }
                }
            }
            else if (isClient && isVerify && NetworkManager.Client.isReady)
            {
                if (sendTime < Time.unscaledTimeAsDouble + syncInterval)
                {
                    sendTime = Time.unscaledTimeAsDouble + syncInterval;
                    using var setter = MemorySetter.Pop();
                    if (WriteParameter(setter))
                    {
                        ParameterServerRpc(setter);
                    }
                }

                for (var layer = 0; layer < animator.layerCount; layer++)
                {
                    if (IsModify(layer, out var stateHash, out var stateTime))
                    {
                        using var setter = MemorySetter.Pop();
                        WriteParameter(setter);
                        AnimationServerRpc(stateHash, stateTime, layer, layerWeight[layer], setter);
                    }
                }
            }
        }

        private bool IsModify(int layer, out int stateHash, out float stateTime)
        {
            stateHash = 0;
            stateTime = 0;
            var status = false;
            var weight = animator.GetLayerWeight(layer);
            if (Mathf.Abs(weight - layerWeight[layer]) > 0.001f)
            {
                layerWeight[layer] = weight;
                status = true;
            }

            if (animator.IsInTransition(layer))
            {
                var transition = animator.GetAnimatorTransitionInfo(layer);
                if (transition.fullPathHash != transitionHash[layer])
                {
                    animationHash[layer] = 0;
                    transitionHash[layer] = transition.fullPathHash;
                    return true;
                }

                return status;
            }

            var stateInfo = animator.GetCurrentAnimatorStateInfo(layer);
            if (stateInfo.fullPathHash != animationHash[layer])
            {
                if (animationHash[layer] != 0)
                {
                    stateHash = stateInfo.fullPathHash;
                    stateTime = stateInfo.normalizedTime;
                }

                transitionHash[layer] = 0;
                animationHash[layer] = stateInfo.fullPathHash;
                return true;
            }

            return status;
        }

        private ulong NextDirty()
        {
            var dirty = 0UL;
            for (var i = 0; i < animatorParams.Length; i++)
            {
                var changed = false;
                var parameter = animatorParams[i];
                if (parameter.type == AnimatorControllerParameterType.Int)
                {
                    var newIntValue = animator.GetInteger(parameter.nameHash);
                    changed = newIntValue != lastIntParams[i];
                    if (changed)
                    {
                        lastIntParams[i] = newIntValue;
                    }
                }
                else if (parameter.type == AnimatorControllerParameterType.Float)
                {
                    var newFloatValue = animator.GetFloat(parameter.nameHash);
                    changed = Mathf.Abs(newFloatValue - lastFloatParams[i]) > 0.001f;
                    if (changed)
                    {
                        lastFloatParams[i] = newFloatValue;
                    }
                }
                else if (parameter.type == AnimatorControllerParameterType.Bool)
                {
                    var newBoolValue = animator.GetBool(parameter.nameHash);
                    changed = newBoolValue != lastBoolParams[i];
                    if (changed)
                    {
                        lastBoolParams[i] = newBoolValue;
                    }
                }

                if (changed)
                {
                    dirty |= 1UL << i;
                }
            }

            return dirty;
        }

        private bool WriteParameter(MemorySetter setter, bool status = false)
        {
            var dirtyBits = status ? ~0UL : NextDirty();
            setter.SetULong(dirtyBits);
            for (var i = 0; i < animatorParams.Length; i++)
            {
                if ((dirtyBits & (1UL << i)) == 0)
                {
                    continue;
                }

                var parameter = animatorParams[i];
                if (parameter.type == AnimatorControllerParameterType.Int)
                {
                    var newIntValue = animator.GetInteger(parameter.nameHash);
                    setter.SetInt(newIntValue);
                }
                else if (parameter.type == AnimatorControllerParameterType.Float)
                {
                    var newFloatValue = animator.GetFloat(parameter.nameHash);
                    setter.SetFloat(newFloatValue);
                }
                else if (parameter.type == AnimatorControllerParameterType.Bool)
                {
                    var newBoolValue = animator.GetBool(parameter.nameHash);
                    setter.SetBool(newBoolValue);
                }
            }

            return dirtyBits != 0;
        }

        private void ReadParameter(MemoryGetter getter)
        {
            var status = animator.enabled;
            var dirtyBits = getter.GetULong();
            for (var i = 0; i < animatorParams.Length; i++)
            {
                if ((dirtyBits & (1UL << i)) == 0)
                {
                    continue;
                }

                var parameter = animatorParams[i];
                if (parameter.type == AnimatorControllerParameterType.Int)
                {
                    var newIntValue = getter.GetInt();
                    if (status)
                    {
                        animator.SetInteger(parameter.nameHash, newIntValue);
                    }
                }
                else if (parameter.type == AnimatorControllerParameterType.Float)
                {
                    var newFloatValue = getter.GetFloat();
                    if (status)
                    {
                        animator.SetFloat(parameter.nameHash, newFloatValue);
                    }
                }
                else if (parameter.type == AnimatorControllerParameterType.Bool)
                {
                    var newBoolValue = getter.GetBool();
                    if (status)
                    {
                        animator.SetBool(parameter.nameHash, newBoolValue);
                    }
                }
            }
        }

        protected override void OnSerialize(MemorySetter setter, bool status)
        {
            base.OnSerialize(setter, status);
            if (!status) return;
            for (var i = 0; i < animator.layerCount; i++)
            {
                if (animator.IsInTransition(i))
                {
                    var stateInfo = animator.GetNextAnimatorStateInfo(i);
                    setter.SetInt(stateInfo.fullPathHash);
                    setter.SetFloat(stateInfo.normalizedTime);
                }
                else
                {
                    var stateInfo = animator.GetCurrentAnimatorStateInfo(i);
                    setter.SetInt(stateInfo.fullPathHash);
                    setter.SetFloat(stateInfo.normalizedTime);
                }

                setter.SetFloat(animator.GetLayerWeight(i));
            }

            WriteParameter(setter, true);
        }

        protected override void OnDeserialize(MemoryGetter getter, bool status)
        {
            base.OnDeserialize(getter, status);
            if (!status) return;
            for (var i = 0; i < animator.layerCount; i++)
            {
                var stateHash = getter.GetInt();
                var stateTime = getter.GetFloat();
                animator.SetLayerWeight(i, getter.GetFloat());
                animator.Play(stateHash, i, stateTime);
            }

            ReadParameter(getter);
        }

        [ServerRpc]
        private void AnimationServerRpc(int stateHash, float stateTime, int layer, float weight, ArraySegment<byte> segment)
        {
            if (syncDirection == SyncMode.Client && !isClient)
            {
                using var getter = MemoryGetter.Pop(segment);
                if (stateHash != 0 && animator.enabled)
                {
                    animator.Play(stateHash, layer, stateTime);
                }

                animator.SetLayerWeight(layer, weight);
                ReadParameter(getter);
            }

            AnimationClientRpc(stateHash, stateTime, layer, weight, segment);
        }

        [ClientRpc]
        private void AnimationClientRpc(int stateHash, float stateTime, int layer, float weight, ArraySegment<byte> segment)
        {
            if ((syncDirection == SyncMode.Server && !isServer) || (syncDirection == SyncMode.Client && !isOwner))
            {
                using var getter = MemoryGetter.Pop(segment);
                if (stateHash != 0 && animator.enabled)
                {
                    animator.Play(stateHash, layer, stateTime);
                }

                animator.SetLayerWeight(layer, weight);
                ReadParameter(getter);
            }
        }

        [ServerRpc]
        private void ParameterServerRpc(ArraySegment<byte> segment)
        {
            if (syncDirection == SyncMode.Client && !isClient)
            {
                using var getter = MemoryGetter.Pop(segment);
                ReadParameter(getter);
            }

            ParameterClientRpc(segment);
        }

        [ClientRpc]
        private void ParameterClientRpc(ArraySegment<byte> segment)
        {
            if ((syncDirection == SyncMode.Server && !isServer) || (syncDirection == SyncMode.Client && !isOwner))
            {
                using var getter = MemoryGetter.Pop(segment);
                ReadParameter(getter);
            }
        }
    }
}