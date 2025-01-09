// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-06-09  17:06
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace JFramework.Editor
{
    internal class SyncVarAccess
    {
        private readonly Dictionary<string, int> syncVars = new Dictionary<string, int>();

        public readonly Dictionary<FieldDefinition, MethodDefinition> setter = new Dictionary<FieldDefinition, MethodDefinition>();

        public readonly Dictionary<FieldDefinition, MethodDefinition> getter = new Dictionary<FieldDefinition, MethodDefinition>();

        public int GetSyncVar(string className) => syncVars.TryGetValue(className, out int value) ? value : 0;
        public void SetSyncVar(string className, int index) => syncVars[className] = index;

        public void Clear()
        {
            setter.Clear();
            getter.Clear();
            syncVars.Clear();
        }
    }

    internal static class SyncVarReplace
    {
        /// <summary>
        /// 用于NetworkBehaviour注入后，修正SyncVar
        /// </summary>
        /// <param name="md"></param>
        /// <param name="access"></param>
        public static void Process(ModuleDefinition md, SyncVarAccess access)
        {
            foreach (var td in md.Types.Where(td => td.IsClass))
            {
                ProcessClass(td, access);
            }
        }

        /// <summary>
        /// 处理类
        /// </summary>
        /// <param name="td"></param>
        /// <param name="access"></param>
        private static void ProcessClass(TypeDefinition td, SyncVarAccess access)
        {
            foreach (var md in td.Methods)
            {
                ProcessMethod(md, access);
            }

            foreach (var nested in td.NestedTypes)
            {
                ProcessClass(nested, access);
            }
        }

        /// <summary>
        /// 处理方法
        /// </summary>
        /// <param name="md"></param>
        /// <param name="access"></param>
        private static void ProcessMethod(MethodDefinition md, SyncVarAccess access)
        {
            if (md.Name == ".cctor" || md.Name == Const.GEN_FUNC || md.Name.StartsWith(Const.INV_METHOD))
            {
                return;
            }

            if (md.IsAbstract)
            {
                return;
            }

            if (md.Body is { Instructions: not null })
            {
                for (int i = 0; i < md.Body.Instructions.Count;)
                {
                    var instr = md.Body.Instructions[i];
                    i += ProcessInstruction(md, instr, i, access);
                }
            }
        }

        /// <summary>
        /// 处理指令
        /// </summary>
        /// <param name="md"></param>
        /// <param name="instr"></param>
        /// <param name="index"></param>
        /// <param name="access"></param>
        /// <returns></returns>
        private static int ProcessInstruction(MethodDefinition md, Instruction instr, int index, SyncVarAccess access)
        {
            if (instr.OpCode == OpCodes.Stfld && instr.Operand is FieldDefinition OpStfLd)
            {
                ProcessSetInstruction(md, instr, OpStfLd, access);
            }

            if (instr.OpCode == OpCodes.Ldfld && instr.Operand is FieldDefinition OpLdfLd)
            {
                ProcessGetInstruction(md, instr, OpLdfLd, access);
            }

            if (instr.OpCode == OpCodes.Ldflda && instr.Operand is FieldDefinition OpLdfLda)
            {
                return ProcessLoadAddressInstruction(md, instr, OpLdfLda, access, index);
            }

            return 1;
        }

        /// <summary>
        /// 设置指令
        /// </summary>
        /// <param name="md"></param>
        /// <param name="i"></param>
        /// <param name="opField"></param>
        /// <param name="access"></param>
        private static void ProcessSetInstruction(MethodDefinition md, Instruction i, FieldDefinition opField, SyncVarAccess access)
        {
            if (md.Name == ".ctor") return;

            if (access.setter.TryGetValue(opField, out var method))
            {
                i.OpCode = OpCodes.Call;
                i.Operand = method;
            }
        }

        /// <summary>
        /// 获取指令
        /// </summary>
        /// <param name="md"></param>
        /// <param name="i"></param>
        /// <param name="opField"></param>
        /// <param name="access"></param>
        private static void ProcessGetInstruction(MethodDefinition md, Instruction i, FieldDefinition opField, SyncVarAccess access)
        {
            if (md.Name == ".ctor") return;

            if (access.getter.TryGetValue(opField, out var method))
            {
                i.OpCode = OpCodes.Call;
                i.Operand = method;
            }
        }

        /// <summary>
        /// 处理加载地址指令
        /// </summary>
        /// <param name="md"></param>
        /// <param name="instr"></param>
        /// <param name="opField"></param>
        /// <param name="access"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static int ProcessLoadAddressInstruction(MethodDefinition md, Instruction instr, FieldDefinition opField,SyncVarAccess access, int index)
        {
            if (md.Name == ".ctor") return 1;

            if (access.setter.TryGetValue(opField, out var method))
            {
                var next = md.Body.Instructions[index + 1];

                if (next.OpCode == OpCodes.Initobj)
                {
                    var worker = md.Body.GetILProcessor();
                    var variable = new VariableDefinition(opField.FieldType);
                    md.Body.Variables.Add(variable);

                    worker.InsertBefore(instr, worker.Create(OpCodes.Ldloca, variable));
                    worker.InsertBefore(instr, worker.Create(OpCodes.Initobj, opField.FieldType));
                    worker.InsertBefore(instr, worker.Create(OpCodes.Ldloc, variable));
                    worker.InsertBefore(instr, worker.Create(OpCodes.Call, method));

                    worker.Remove(instr);
                    worker.Remove(next);
                    return 4;
                }
            }

            return 1;
        }
    }
}