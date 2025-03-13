// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-07 20:12:02
// # Recently: 2024-12-22 20:12:33
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

namespace JFramework
{
    internal static class ProjectIcon
    {
        private static readonly Dictionary<Icon, Texture2D> IconTextures = new Dictionary<Icon, Texture2D>();

        private static readonly Dictionary<Icon, Lazy<string>> IconStrings = new Dictionary<Icon, Lazy<string>>
        {
            {
                Icon.Animations,
                new Lazy<string>(() =>
                    "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAADC0lEQVR4Ae1bvYrbQBCWZIGNCifkCQIpw+UFDu6KFKkuRRJIlVdI5S71da7uIXJXXJUuZQIp3MZdIHB9CoMNNv6RlW98NkiL8MyedCuxu4JF2t1vdmZnZ2Z/JIXj8Tjgrvl8HqRpGmy3Ww56tJ7oe71e0Ol0SnGLxaK0XFqYZVkQRVEQhuGOR5IkLGnEIiwHeAVYPsBs97wFsCqyHOAtwPIBZrvnLYBVkeUAbwGWDzDbvZhF3ANe4HaJdIZEC+wMSfc6WNsnEH7TJX4svEQBz5fL5W8IkNBGo8pFmxS0dY1NyjO0s6zSVl208XQ65dq6AoA6n3JArh4KCLEjTKCEr3Ecv8/vCmnHSQoyfUW0hWTSedWRz3UqQ1vZZrN5h7KLXHljjxKblmB0OrDdu8INiLo6hI+BlXRuUzdj8gRY3c4V6JBkvV7XzULcnkQB4sY0gK1xBcksoNEvLSi5Qme1Wt0gGJ6A8p8WdTmYoiidq4lNqkkFUNRPyRXgAn/wPCvvk7wUbYRob47bT1B9QfrLUTeqgL1wKQSmU9KnnLDC+ieYZT4iXWC98RI0d8fo2qAAkq/yGiPfSZq2ab0xm81oDfM2X6c+t0UBqlyV83slnHMNGVfAYDAofynASSqsHw6HeWtiZzkWIOTbVhi7hrFdAezAOK8A4zFAHZI3v27VIq3899MPBbwSY9ip1XkL8Aoo2I+DmcZjgAGdq+eXhWMn7wIGRqDVLJy3AOtigLquUNcJMMdCTHDeArwCWh2hDAhnXQxQfZ6LCd4FDFhZq1k4bwGNxwDVZ3XNhfNxrn3nLcArQGByjbuJQMYHQySdq/aTwINFuyfU9XEOr4oT4b3c7tOUI/cf9A7f1ivu9/tc3z5PJpPX9K6txk9lOJ7G6mP6OIm57rrd7iv8MnMJJVT5TO7Ahj2qPgBN3MPRaCTiU8c/Q8QI5/Ycv8J+XQXr+jiH99OgqmHX8s5bgGQdYNooCuf2YF6ICdzaXhWWwztvAV4Bqsm4lm9jDFDHQI0Jan2lvHeBSuqzgNh5C/gP+JneGWNBOPcAAAAASUVORK5CYII=")
            },
            {
                Icon.Audios,
                new Lazy<string>(() =>
                    "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAADFElEQVR4Ae1bvY7TQBC2HUuJXATEEyBRIhD9SXcF9VFAAQ2vQJUGUZ9oUvEQcPW9waHQI9IhIV1PESmREuXH8X0TEmkdOTuztz7HrHellde7M7M7387srx0Oh8OAC9PpNEjTNFiv1xyptpz4O51O0Gq1Culms1lhvjQzy7IgiqIgDMNNHUmSsKwRS+E4gQfA8Q5m1fMWwELkOIG3AMc7mFXPWwALkeME3gIc72BWvZil+EfwBI8LxFNEWmBniKZhZ23vwXhlynxf9BIAHs/n819oQEIbDZtAmxTI+oZNyiPImdvIKos3Ho/HnKwvICDlU46QKwcAIXaECUD4GsfxG3VXSDtOAqjqENEWkolntj2vKJVBVrZarV4j71zJP1pSYtMSGhMF1ltXuART24TxPmglyq3Krpg8AVa3cQU6JFkul2VXIZYnAUAszICwNq4gmQUM9DIiJVdoLRaLSwyGz8D514i7mJhGUTpXE5vUMQGgUT8lV4AL/EZ6UqyTPBcyQsib4vEdXJ8Q/3DcRwVg27gUDaZT0odcY4XlDzDLvEU8x3rjKXhudHx1AIDaZ73GUJWkaZvWG5PJhNYwr9Sy/XRdANhvl/X7FoQzTlDtAOj1esWXBgc06ff7OuvRznKoK9ASHKjzf8o+uIYh5Sm4DkBhZ+2UbyQAqvIEQDgYDApRUjJHSNMUpfM1hdwsiQZpff7H5585gScfX+TeBS/aLWYjXUAFzQOgotHEdOXrAM7nTTtBMEZozy+9C5gi7hp94y2g8jFg34IEPrzPYvTOyW+8BXgAjOzJQWJvAQ52qpFK3gKM4HKQ2FuAg51qpJK3AAFcR18uC9p4ZxKJcnY/CTBNu8MZHyMxX8zJj3Avt/k0RfO8pjt8V0Pc7XY53T6MRqOXdNdW4qcyXJ2Vlcf0cRITbtrt9nP8MnMBEGw+k9tVU9Yt8E6e1VMyBlAFdM/+zqomOXPuDI/zYYFYfy+gA8mvA3ToNKFMOgZUiYXWZ9GQUscI7wJVdm0d62q8BdRxDOAMhRsjOP5ceeMtwAOQs4cGvtwCHonPi2L0I/AAAAAASUVORK5CYII=")
            },
            {
                Icon.Editor,
                new Lazy<string>(() =>
                    "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAADB0lEQVR4Ae1bvW7bMBCWZAE2NLhFnqBAlwJFsnRvhi5dkiEZ2qWv0Mlb57xAHyLJ3Clrunepl6JAgewdDNiADf/I6p1rI8JB4B1BxqJJEiAkkndH3se7oyhK6XA4TLg0nU6TsiyT9XrNkSrbkb/X6yWdTqeRbjabNdZLK6uqSrIsS9I03fRRFAXLmrEUnhNEADyfYFa9aAEsRJ4TRAvwfIJZ9aIFsBB5ThAtwPMJZtXLWYr/BC/hcgX5LWR8wK4g66adtX0Cxm+6zE9FLwHgxXw+/wkDKHCjYZJwkwKybmCTcgRy5iaybPHm4/GYk/UVCFD5kiPk2gGAFHaEBYBwnef5ZX1XiDtOBGjfKcMtJJNPTWe+plQFsqrVanUBdWe1+tZuJTYtodFRYL11hVtg6uowPgWtRLmV7Y7RE8DqNq6AL0mWy6XtLsTyJACIhWkQOuMKklVAQy8tUnSFzmKxuIVgeAycf7W4m4kxiuJ7NbFJtQkARv0SXQFc4DfcT5p1kteCjBTkTeHyHbi+QP7DcbcKwHZwJQwY35I+5wYrbH8Gq8wHyGfwvPEaeB5UfC4AgOMzfsaoK4nLNj5vTCYTfIY5r7fRe1cAoOMyLm9BOOUEOQfAYDBoPjTgNGlu7zdXP9a2tQw+jqDluwhAyxPQevfOxQCKyN2rN7RKWX7/64eynTZGF6CIhFYO3gL2HgNM13nq44IYoXx/GbwFRABCC3pU373HADoAgQ9TFmWZk0djSHQBJZwBNAZvAa3HAGpk1EdpOy1z9FxMCN4CIgDUpEIrOxcDOJ+lPs/RcxMaXYBDyPd2iQU45yY2J0WinNlPAsxoqU9Tcl0f5+RR+Rmcy20+TVFc7/EM39eU9/vs4cnn0Wj0Ds/aLH4q4wyeOX6cxKSHbrd7Ar/MXAEIJp/J7bqxdQq8k2d0lcQA7ADP2T8a9SRnVr7D040J0K3y0zPJKiAf+gFSRgAOcNKsDlkaA6x2qiNMd13XkY200QV0EfONPngLcDEGKNdt2xYYvAVEAGyb1KHJ+wcUyMtZX8hxwgAAAABJRU5ErkJggg==")
            },
            {
                Icon.Lights,
                new Lazy<string>(() =>
                    "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAADRUlEQVR4Ae1bP4/TMBRP0kitOhTEJ0BiRDAzcQPzMXADLHwBBqZuzLd1YuAjwC0sfANQB2a6IZ10O0NFK7XqnzT8Xigo9qV+9jlNcrEt+VL7/bHfz+/5T5wLJ5NJwKXFYhEkSRLsdjuOVUkn+V6vF3Q6nUK+5XJZWK9bmaZpEEVREIZh1ka/32dFI5aj5QwegJYPMGue9wAWopYzeA9o+QCz5nkPYCFqOYP3gJYPMGtezHL8ZXiAxznyU2TaYKfIpumft72G4BdT4WPx6wBwf7Va/UAH+nTQsEl0SIGuTzik3IOelY2usmTj2WzG6XoPBjI+4Rg5OgAIcSLsA4SPcRyf5U+FdOIkgKpOER0hmXxiO/I5o1LoSrfb7QvUnebqa/up49M6PCYG7PahcAGhrongMXh1jNuW3TBFArwuCwV6SbLZbMpuQlufDgDaygwYGxMKOquAgV1GrBQKnfV6fYHJ8BEkfxlJFzPTLErv1bRdqk4AaNZPKBQQAj/xe15sk34tdITQt8DjG6TeIV9y0rUCsO9cgg7TW9K7XGc16XewyrxEPsV+4yFkrlRyTQCA+me9x8gbScs27Tfm8zntYZ7nafLvpgAg98u6vAfhhFPUeACGw2HxJcIBy0ajUd6bhFUOuq5JCQzXqLe/4v8epsh4Mq/tAGRDeMh4JwBQGU8AhOPxOENJ8WcKGi1R+dhSsJuR0EGjGP/84bvQwNmbJ0JZoyAcOZ0IARUoHgAVOi7QGrcPsI1xU3kfAi64ucpG5z2g8n2A7bqvGk2ime4LnPcADwDnUm2n174PkNdtW8A5ffIc4UPAFvHbLu+8B9Q+B8gxKXsUF9MyP6dP5nfeAzwAsksUlGsPk4I+lValcxb4jdYGyJW8E5RjXo5pWzrsEN8J4l4u+zRF8fxKd/htTfFgQIOrTG+n0+kzumsr8VMZZYNVEmP6OIlJV91u9zH+ZeYcINh8JneombJuhQ/pV9brTnCX0PJKqekGxP2lhfDNoRzzpmpN5WtbBrkbG1PDb8pfCwBNMZ5AqxyAJhlPAOjOAcR7rCSsywWNmM4RnD6hico9QGi9AQUPQAMGodYuNGEO4AAwimlOmUz3ISAj4lrZeQ/4Ay9u7jvSr9GrAAAAAElFTkSuQmCC")
            },
            {
                Icon.Fonts,
                new Lazy<string>(() =>
                    "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAADP0lEQVR4Ae1brY/bMBRP0kitArpptGTS4LT9CXdgrNINbGAjg6VDZcPHikbLd4cHynes9MomTTpSNFCplVr1I82913VSbDl5dj4cX2JLVmL7vWe/n997sePEnc1mDpXW67UThqFzPB4p0tR25O90Ok6r1RLSbTYbYb1sZRRFjud5juu6pz6CICBZPZKi5gQWgJpPMKmetQASopoTWAuo+QST6lkLICGqOYG1gJpPMKmeT1L8I3gFl2vIF5BxgR1BVk3/re0LMP5UZS6LXgaAl9vt9h4GEOBGI0/CTQrIuoFNyguQs80jqyhef7lcUrK+AwEqH1KEVDsA4MKOMAAQfvi+/zG+K8QdJwKkO3m4hSTyZd6ZjykVgazocDh8gLqrWH1ltzI2LUOjosDx7Aq3wNRWYSyDVka5Q9EdoyeA1Z1cAV+S7Pf7oruQlicDgLQwBUJjXEHmKaCglxIpukJrt9vdQjB8A5x/lbjFxBhF8b2atElVCQBG/RBdAVzgN9yvxDrJ14IMF+St4XIHXN8g/6G4KwXgPLgQBoxvSZ9Tg5VsfwZPmU+Qr2C98Rp4HtL4TAAAx5d7jRFXEh/buN5YrVa4hnkfb+PvTQGAH1fu8hmES0qQdgCGw6H4UIAaabb2LsVW1WOQGpe2dguANqgN7Uh7DOBxmEwmfFWucr/fV+K3LqAEVw2JG28BlccA3md7vR5jZ+PxmCkPBgOmPJ/PmbKgwL+/ZF47Nd4CLAACk2lUlfYYMBqNmJ1f0XsDfl3BxxiYXSYmWBdolL0LlG28BWiPAYJJyFXF+zwvjG/nY0LjLcACwJtM08pPPgbwPk35PD/B1gV4RJpWlrGAJ+8maZMqA0C+nwTSejegzYNzudOnKSnXX3iGX9fkd7vk4cnXxWLxDs/aCvxUxhg8ffw4iUgP7Xb7Lfwycw0g5PlMLqmbok6Fk+Sn1ssGODxn/5wqKUMjvAtALmZ/nkFMKgu1LpAJgqkdZG08K5+VvTC+SgAwRXlEUTsAJimPALjT6RSvZCriv8EE5UuNAZRi2iwgQXlqfKW3awHAVOUR3dIBMFl5BEB2HYC0ZSXmrK6sTpLklm4BSR2bUm8BMGUmqhrHIwTF4P2KpsEpAAAAAElFTkSuQmCC")
            },
            {
                Icon.Materials,
                new Lazy<string>(() =>
                    "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAADSklEQVR4Ae1bMYvUQBROsoEsKdbj8AcINoIoWFgdeIKF1SlooY2NtVjIYmN9jYXFYW2p12pnp4KVheA2ciBcr7iyC7tkd7PxvXX3LvPI7ZtxspvczARC5s2892bmm/fezGQSv9PpeNw1GAy8NE296XTKsS4tR/lms+k1Go1CvuFwWJgvm5llmRcEgef7/qyOOI5Z0YDlMJzBAWD4ALPdcxbAQmQ4g7MAwweY7Z6zABYiwxmcBRg+wGz3QpbjH8N5eOzCfQ1uXGBncKteC2t7AILvVIVXxS8DwLkkSb5BA2LcaOhcuEkBXW9gk7IJehIdXWXJhr1ej9O1BwzY+ZRj5MoBAB92hDGA8DoMw7v5XSHuOBGgdV8BbiGZe1t35HOdykBXNplM7kDeTi6/sqSMTcvwqHRgOneFfRCKVARXwSvTuUnZFaMngNXNXAFfkozH47KrkNYnA4C0MgXG2riCzCyg0C8lVnSFxmg02odgeAkkfypJFzNjFMX3atImVSUAGPVTdAVwgQNI94v7JJ8LOnzQN4DHJ5B6BvcPTrpSAOaNS6HB+JZ0g2usZPkZmGXuwb0D642LIHO4TK4OAGD7tNcY+U7itI3rjX6/j2uYW/kymq4LALRd2vQchG1OUe0AaLfbxYcGXE+Ky1vF2ce5VU2Dxy2oOOUAqHgAKq++8hhAfT7aOCuAcv3FW4HmiA9PbgssyZ9f9N2FsOV0LiDAZSFhvQWsPQbo+vzv718FO/3y/JFA33z1WaC5mGC9BTgABHuxkFh7DKAYq87zmxeuCCquPn0p0O8fbgk0jQm03LmAAJeFhPUWUHkMoEZH53nq85Rfl7beAhwAuiZ02uVrFwPo2p7O8xRwyk/LOdq5AIeQ6eUyFlA7NylzUGQ6p/eTANNaul/n1u6MOo/KU/1UPoBzudmnKUueH/EM39QrbLXYw5PH3W73Bp61lfipTG3wDPHjJOY6jKLoMvwyswsg6Hwmt6imrFPghT6tp0wMwArwnP2+Vk0nCNP39tRnqU+foOYom8qD/qOyecKdC+QRkZkG8/zGpa0HQDYGrHLkBZ+kMYG+w/uPhgj6qbz1FuAAoCZhG12HGEAxX+qzlFmXdi6gi+Bpl7feAv4C9fjrJoJRvwIAAAAASUVORK5CYII=")
            },
            {
                Icon.Meshes,
                new Lazy<string>(() =>
                    "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAADUElEQVR4Ae1bP4/TMBRP0kitMhQEIwsSIzo+Ajcwn4RggIWxK0PVjfm2TvchjhsR3wC2rnRDQrqFCaFKrdSqf9LwXtXeOVbqZ2PHSWNbsmL7/bHfL+85dpyE4/E4oNJ8Pg/SNA222y3FKqSjfKfTCVqtViHfYrEobJdtzLIsiKIoCMNw10eSJHeivV7vrswWIrbiYtkD4OJdZ232HsCi4WLZe4CLd5212XsAi4aLZe8BLt511uaYrQjKz4B2CfklZFxgZ5BV08HbPoDgV1XhsvhlAHi6XC5/wAAS3GjoJNykgK7PsEl5BHqWOrpMycbT6ZTSdQUMaHxKMVJ0ACCEHWECIFzHcfyW3RXijhMBsp0i3EIS+Vz3zjNGZaAr22w2b6DtgmmvrCjj0zI8KgZs96FwA0JtFcEyeGWM25juGCMBvG4XCviSZL1em+5CWp8MANLKFBhrEwoyTwEFu5RYMRRaq9XqBibDM5D8oyRdzIyzKL5Xk3apKgHAWT/FUIAQ+AnlWbFN8q2gIwR9c7h8B6lPkH9R0pUCsB9cCgPGt6QPqcFK0h/AU+Yd5AtYbzwHmVuRXFVzAD8mXGMYy/s1SzKbza4gB8feCOMg6gIAD4h2Hdcu8IQ57/f7Ql3WQ2AwGBQfCgiHeU8cDoe5FSmhrwuSwn1LYz3gHjJxyQMgxqf5VOtzAA/pky+P+aZc/ffrv7k6EfOBqj4fAjl4Haw47wGVzwGU01ExTclTdOc9wANAuUjT6ZXPAfxzno95nk7dEFV5HwIUok2nO+8B1ucAxf18GQ6YO35y3gM8AGX42CnptD4HUPt5C+Dl3hH6ELCAeK27kPEA62FiEzEZ4/R+EiCs4dfuPDtF5/n5Oi/P7y0iOJfbfZoiuH7DM/ymprjbxcMTYfo4mUxe4bc9Bj+VEXZokxjjx0lEum232y/gl5lLAEHnM7lDN6ZOgQ/6tK4ycwB2gOfs77V6AmFYA6CK3HOY0snHLMXPxzzFL/MUoHRI0ffGS/HaZLICQF2NR6BLB6DOxiMA4Wg0wiuZ/ue/wSPGK80BBQPL7eeBrqWvNA84YnyBPdU2lQLAqRiP0BsH4JSMRwBk1wHIayrxMayrV0ufcQ/Qtca2vAfANuJ16+8fsDr9ofTxIJkAAAAASUVORK5CYII=")
            },
            {
                Icon.Physics,
                new Lazy<string>(() =>
                    "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAADJklEQVR4Ae1bO4/TQBC2HUs5WSIg/gBIlOj4CZwEFcU1UFBRpqUI6aivpLkfAVfT0CFBl5Z0SEhHTREpkRLl4ZhvouS0XpndWbx+XHZXWnkfM7M7n2fGu147HI/HgS7N5/MgTdNgu93qSJX9xH9ychJ0Op1CusViUdjObcyyLIiiKAjDcDdGkiQ3rP1+/6YsFiKx4mLZA+DiXRd19hYgouFi2VuAi3dd1NlbgIiGi2VvAS7edVHnWKwoyo/Qd4H8FJkW2BmyaTpY2xswfjZlroqeA8DD5XL5AxNIaKNRJtEmBbI+YZNyH3KWZWTZ4o2n06lO1iUISPlUR6jrBwAhdoQJQPgYx/ErcVdIO04CqO4U0RZSk8/K3nlBqQyyss1m8xJt50J7Y0WOTXNoTBTY7l3hCkxdE8YqaDnKbWwPTJ4Aq9u5Ar0kWa/Xtodgy+MAwBZmQNgaV+A8BQz0MiIlV+isVqsrBMNTcP4x4i4mpihK79XYJtUkABT1U3IFuMBPlGfFOvFbISOEvDku38H1HvmXjrtRAPaTSzFhekt6TzdZZv9dPGVeI59jvfEYPNcqvqZigDwnWmNYy/s1SzKbzS6Rg3+9EaZJtAUAGZDSdVq74AlzNhgMlLJqd4HhcFh8KKCc5n939sCp3LccrQVwIfMAcJE6VrraY4AM5LuvD+QmZf3Ds9+5/rL83gVycDpYcd4CGo8BstGNv+S3BKcv7sgkyropv/MW4AFQ2pMDna2LAaY+L98jU37vAjKCrtWdt4DWxQDba33dXsF5C/AAuBb0ZH1bFwPkCcp1nU/L9Lq6dwEdQsfez7GAW+cmJjeNo1y5nwRMZlNAK68LCkhKNUU4l9t9mqK4fqMz/GNNca9HhyfK9HYymTynb3ssfiqjHLDOzpg+TtKk6263+wS/zFwAhDKfyR2GsXUKfJBX6hqORiOWABv/DOFckMZSntXZfs7rYgjnKcACSEe0V15HVnt/LQC0VXlCu3IA2qw8AcBZBxCdzSR/DpqLCTqftTCR3PiVW4CFCVcqwgNQKby3QHgTMUCGJeeTcmfVde8CVSPcdvnOW8Bf2T3uOY/IghYAAAAASUVORK5CYII=")
            },
            {
                Icon.Plugins,
                new Lazy<string>(() =>
                    "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAADJUlEQVR4Ae1bP4/TMBRP0kitMvQQnwCJEcFH4AZ0G7fAwMTA0JWh6sZ8X+C+ABvcjNiYuK0r3ZCQbmeo1Eqt+ifN/V7VnhwrzbMvfxzFtmTVzvvn98t7jh2n/mQy8biyWCy8OI693W7HsebSSb7X63mdTieTb7lcZl5XvZgkiRcEgef7/t5GFEUPooPB4KEtNgKxY2PbAWDjXRd9dhEgomFj20WAjXdd9NlFgIiGjW0XATbeddHnUOzktJ+DdoX6GpUW2AmqbjlG20cI/tAVropfBYBnq9XqDwYQ0UajSKFNCnR9xyblKfSsiugqSzaczWacrmswkPMxx8jRAYCPHWEEEL6FYfhe3BXSjpMAqrsEtIVk6nnROy84lUBXst1u3+HapXDdWFMlplV4dBzYHVLhBkJdHcEqeFWc25ZtmDIBUbdPBXpJstlsyjahrE8FAGVlGoyNSQWVp4CGX1qslAqd9Xp9g8nwJST/a0lnM9MsSu/VlEPKJAA068eUCkiBv2jPs31SvwodPvQt8HMLqS+o/zhpowAcBhdjwPSW9Ak3WEX6GZ4yH1Avsd54AZm7PDlTc4A8JlpjlFYPa5ZoPp9fo3qn3gjTIJoCgAxI4T6tXfCEOR8Oh7m6jKfAaDTKPiTIHbYysQ9Oed+SWm62NgJUIXIAqCLVVr7a5wAu5z+9/VUq1l9/Xsj6UnOCSwEZHtv61kdA7XOAHGFyzmfkrCyi1ef0Wx8BDgCteGohs/E5gMOUy2GOzul3KcAh1Ha6i4C232HOPxcBHEJtp1sfAY1fB3B7A47ORbD1EeAA4EIE9ManiYIPJ1lUnCv2J4GTph9HKLr2l60GOJfbf5qS8/ubzvDbWsJ+nw5Pcsvn6XT6hr7tKfFTmVyDdRJD+jiJKXfdbvcV/jJzBRCKfCZ3NFPWKfBRX6FffzweKyko4z9DOBMgW6n38rJxOcdluvzc1+WX9dX2GDw4L9s33q8FgKY6T+hXDkCTnScAVNYBxFdmSZ3PQ3FqTpBznDOsyw99KfuVRwDngGm6A8D0HTBt38QcIPucykmZWHXfpUDVCDddv/URcA+ajvRAAAh6WQAAAABJRU5ErkJggg==")
            },
            {
                Icon.Prefabs,
                new Lazy<string>(() =>
                    "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAADAElEQVR4Ae1bvW7bMBCWZAE2NLhFn6BAxqJ5gQDN0KFTWqAd2qVLHqCTt855gTxAxjRru3Vsgb5AvRUIkL2DARuw4R9Z+c61EZFQfMdSthSRBAiJ1N2R9/HuxB8p7Pf7AZfG43GQpmmwXC450q3Pib/T6QStVquQbjKZFNZLK7MsC6IoCsIwXLWRJAnLGrEUDSfwADR8gFn1vAWwEDWcwFtAwweYVc9bAAtRwwm8BTR8gFn1YpbiH8EBLmfIL5Bpgp0hm6aNtX0E4zdT5l3RSwB4Op1Of6MDCS00bBItUiDrCxYpTyBnaiOrLN54OBxyss5BQMqnHCH3HACEWBEmAOEyjuN3+VUhrTgJoH2niJaQTD62HfmcUhlkZYvF4i3qTnL1ld1KbFpCY6LAcu0KV2BqmzDuglai3KLshskTYHUrV6BNkvl8XnYTYnkSAMTCDAhr4wqSt4CBXkak5Aqt2Wx2hWD4HJx/jbiLiSmK0r6a2KSqBICifkquABf4g/tRsU7yWsgIIW+My09wfUa+5rgrBWDduRQdpl3Sx1xnhc8f4S3zHvkE841n4LnZxlcHAKh/1nOMvJL02qb5xmg0ojnM6/wz/b4uAOj9si6vQTjmBNUOgF6vV3xowGlS/LxbXH1XW9Vr8K4HFd95ACoegMqbrzwG6D7/6uJXqaB8Pz3S9y6UJad3gVLhfoDCnLeAvccAzufhs6Xa0cGbU0Xe9dcLJSY4bwEeAMU+HCzsPQaYYlz2vAAxQOmCdwEFDgcL3gIcHHRFZW8BChwOFrwFODjoisreAhQ4HCxILKD202WbcZMAYPeTgE3v9sAb4Vxu9WnKlusPOsNvaoq7Xfbw5NNgMHhJZ20lfipTGzxj+jiJSTftdvsQv8ycAQSbz+Q2zZR1CryRZ3WVBjg6Z/9g1dI9zPq+vb6Hdw+buLpgj9GfC+TRk7wF8vSNu3ceAGkM2OXIKz6p79vre3j/0RFFvs7vvAV4AHSTcK1chxigY77VZ3Vi27J3AVsEHzq/8xZwC3jeyGEe+zWaAAAAAElFTkSuQmCC")
            },
            {
                Icon.Project,
                new Lazy<string>(() =>
                    "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAADYElEQVR4Ae1bvasTQRC/Sw4Srogi/gGCjSDa2vkKG5vXaKGFlmmfENJZvy6N728QfbWVrVamNY0Iwut9EEggIR+XcybEx+6Q253NfeyR3YUl+zEzu/Pbmbnd20s4Go0CXZrNZkGSJMFms9GRKvuRv91uB81mcy/dfD7f285tTNM0aDQaQRiG2zHiOL5h7Xa7N2Wx0BArLpY9AC6uuqiztwARDRfL3gJcXHVRZ28BIhoulr0FuLjqos6RWFGU70PfOeSnkHGDnUI2Tf+t7S0wfjFlLoueA8C9xWLxEyYQ40EjT8JDCsj6DIeUOyBnkUdWUbzRZDLRyboAAlQ+0RHq+gGAEE6EMYDwKYqil+KpEE+cCFDVqYFHSE0+ybvyglIpyErX6/ULaDsV2q0VOTbNoTFRYLNzhUtgapkwlkHLUW5d9MDoCWB1W1fAlySr1aroIdjyOACwhRkQ1sYVOE8BA72MSNEVmsvl8hKC4SPg/GvEvZ8Yoyi+V2OblE0AMOon6ArgAr+hPN2vE78VZIQgbwY/34HrPeQ/Om6rAOwml8CE8S3pbd1kmf234CnzCvIp7DceAs+Vis9WDKBzwj1GYXm3Z4mn0+kF5CDrjTBOoi4AUEBy13HvAk+Yk16vp5Rl3QX6/f7+SwLltNmdHaCk5xZpu3m0FsCFyAPARepY6azHAArsg3dfaZNU//XhuVQ3pZeYoeJdgCLiWt15C6hdDKAWSH1e16+LCZTfeQvwAFCTcK1eeQwoee/PWT/pbOBdgAPZMdM4bwGVxwBqTfS5fT38SEmM6pSfyqf7CuctwANgZF9HSGw9BlCfpD57/cMsJtx98kZaJipf6oSKdwGKiGt1jgVYd5MyF4WjXL4/CZDZDwYD6UsTC2cD+V4A7uW2n6Yofr/hHf6xpqjTwcsTZTobj8fP8NueAj+VUQ5YZWeEHydp0lWr1XoMf5k5BxDyfCaXNUxRt8JZ8pXtnBiAAvCe/bVS0gGd4P/IJZ3PqRi6L6DPddpP+XV1zlNAJ+Og/p3yB/EWyWQFgLooj0BWDkCdlEcAwuFwiL/aVMT/BjOUV8YA7cTMCeR9gDn/YRwZyh8mrECuSlygrsojjqUDUGflEQDuPgBpy0qST5Y1SJbc0i0ga+C6tHsA6rIStubxDz24AEfSLxofAAAAAElFTkSuQmCC")
            },
            {
                Icon.Resources,
                new Lazy<string>(() =>
                    "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAADNklEQVR4Ae1bvW7bMBCWZAE2NDhFn6BAx6J9hGbInMUdOhgdvXYwvHXOC+Qd3Gaxh75Bs3mNtwIFsncwYAM2/COrd4adUAeJOlWUxJgkQIg/d0fep7sTKUrudDp1stJyuXTCMHT2+30WqbQf+VutltNoNBLpVqtVYju3MYoix/M8x3XdwxhBEDyx9nq9p7JY8MSKiWULgIl3XdTZWoCIhollawEm3nVRZ2sBIhomlq0FmHjXRZ19sSIpv4W+G8gfIeMCO4KcN52s7Qsw/szLXBY9B4A36/X6ASYQ4EajSMJNCsj6AZuU1yBnXUSWKl5/Pp9nyboFAlQ+zCLM6gcAXNgRBgDCd9/3P4m7QtxxIkBVJw+3kBn5suidF5SKQFa02+060HYttNdW5Ng0hyaPAvujK9wBUzMPYxm0HOV2qgdGTwCrO7gCviTZbreqh2DL4wDAFpaDUBtX4DwFcuiVixRdobHZbO4gGL4Hzr+5uJOJMYriezW2SdUJAEb9EF0BXOA3lBfJOvFbQYYL8pZwuQeub5D/ZHHXCsBxciFMGN+SvsqaLLP/Ap4ynyFfw3rjHfA8yvjqigF0TrjGUJaPa5ZgsVjcQnbS3gjjJHQBgAJSuI5rF3jCXPb7famsyl1gMBgkHwpIp/nfnW3glO5bztYCuJBZALhInStd5TGAAjkcDmlTqfVutxuTb10gBoeBFeMtoPYYQI2O+iiNEaPRKMYyHo9jdVqh/LTfeAuwAFCTMK2uXQygN4DGhE4H36c+J+rjlP6ZMrlkXSAZF3NajbcA7WMA9XG6Dsjr89S2jbcACwA1CdPq2scA6uN2HaDYRG0MYACqvZswdEgl4ShX7CeB1KF5HaWvA+Bc7vBpiuT6C8/wzzX57TYenkjT19lsdoXf9ij8VEY6YJWdPn6clJEem83mB/hl5gZAKPKZ3GkYVafAJ3mFru5kMmEJUPHPEJwL4lixszrq46zJFCCi64rKHoNH5QtMvRzWSgDQVXmEtHQAdFYeASg1BqQoH4sBOIk6U2kWkKJ8nbomjl0KAC9FeUREOQAvSXkEgLMXQDqVqfpPwiWzV24BkrG07LIAaHlbKpzUP/NIBGX2TvsrAAAAAElFTkSuQmCC")
            },
            {
                Icon.Scenes,
                new Lazy<string>(() =>
                    "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAADPElEQVR4Ae1bvY4TMRDe3ayUaJEC4gmQKBE8AldQXwMS0FCmoKGI0lHfC1zDG8CJkjcApUlLOiSk6ykiJSJRfjZ730TJybES/5y9u77Ylqy11zPjmc8zXnu9Gw+Hw0iWptNplOd5tF6vZaTCduJvtVpRo9E4SDebzQ7eV71ZFEWUJEkUx/GmjyzLblk7nc5tmS0kbMXHcgDAx1FnbQ4ewKLhYzl4gI+jztocPIBFw8dy8AAfR521OWUrgvJTtF0gv0SmBXaBrJt23vYBjD90mcuiVwHgyXw+/w0FMtpomCTapEDWN2xSHkPO3ESWLd50PB7LZF2CgIzPZYSydgAQY0eYAYSvaZq+YXeFtOMkgKpOCW0hJfnMdOQZowrIKlar1WvcO2fu11ZU8WkVGh0D1ttQuAJTU4exDFoV41a2O6ZIgNdtQoFekiyXS9tdKMtTAUBZmAahM6Gg8hTQsEuLlEKhsVgsrjAZPgfnPy3uw8Q0i9J7NWWXqhMAmvVzCgWEwB+UJ4dtUr8LGTHkTXH5Ba7PyH9l3LUCsFUuh8L0lvSRTFnF9od4yrxDPsd64xl4rkV8dc0BvE60xrCWt2uWbDKZXCJHx94IkxKuAMADYlyntQueMGfdblcoq/IQ6PV6hw8FhGreubENTuG+5WQ9QBWyAIAqUqdKV/kcwAP5/cuDvVtvP/7fq5fdHkJgD24PK957QNzv92XjPgIBLVON3whRRxWvA6hLYfLeAwIAQv/woNG5dUDZmPPrjBACZSPuunzvPaD2OYD3ED5G+XbdOr+X4Pm994AAAO8SvtWdmwP4AZDFME+vO4eEEOAR9K2u4gHOh4nJoKkYZ/aTgIl24NWNad3uEpzLbT5NEVx/0hn+qaa03abDE2H6NBqNXtG3PRY/lRF2WGVjSh8nSdJ1s9l8gV9mLgCCyWdyu25snQLv5BldVeYA6oDO2d8b9QRmvA8kEcKzOt3nvqlOKk8B0z42/FvjrciyKaQSAFw1noAsHQCXjScA4sFgQFdpust/g0eMF84BUkUsE5TmAUeMt6y+ubhSALgvxhN81gG4T8YTAKrrAKK1lar/JFyguXUPEPTlZFMAwMlhqVCpG/7198oIy7XGAAAAAElFTkSuQmCC")
            },
            {
                Icon.Scripts,
                new Lazy<string>(() =>
                    "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAADE0lEQVR4Ae1bvW7bMBCWZAE2NDhFn6BAx6LduzRD5nRoh3bpK2Ty1ozNC+Qhksx9gxYGCq/xFqBA9g4GbMCGf2T1zrUT+UDojiBjqSQJEBJ5P+R9PP5L8XA4jLgwnU6jPM+j9XrNsVbSUb7T6UStVkvJN5vNlPnSzKIooiRJojiON2VkWcaKJiyH4wwBAMcbmDUveAALkeMMwQMcb2DWvOABLESOMwQPcLyBWfNSluMfw0t4XEB8BxEX2AVE3bDzti8g+F1X+Kn4JQC8mM/nt1CBDDcaJgE3KaDrGjYpz0HP3ESXLdl0PB5zui6BAY3POUaODgDEsCPMAISrNE0/lneFuONEgA4dEtxCMvHYtOVLRhWgq1itVh8g77SUX9urxKclPDoGrLdd4QaE2jqCT8ErMW5lu2DsCeB1m66AhyTL5dJ2EWJ9EgDEyjQYG9MVJLOAhl1arNgVWovF4gYGw9cg+UdLWs2Moyieq4ldqk4AcNTPsStAF7iD94naJnku6IhB3xQeP0HqK8TfnHStAGwrl0OF8ZT0GVdZIf0IZplPEE9hvfEKZO6r5JoAANbPeI1RNhKnbVxvTCYTXMO8L9Poe1MAoPUyTm9BOOYUNQ6AXq+nvjTgLFHTu+rsx9y6psHHGtT8FgCouQFqL75xYwBF5Ozbr72sy/O3e2mOvsesSIQuoADFqyzvPeDgY4DleV7irZXnl957QABA4kMu88T9fp+zbwQMuFW1smOjYwCdx7nKmNLpOiJ0AVNE/3d57z3g4OsA6jG0T1I6HSMoP0en+mjaew8IAFCX8C0d1gG+tTi1N4wBFBFFuvapUlEna1kS48x+EmCqSud1ys7N8xyd6qPpBO7lNp+mVDx/4B2+qyHtdtnLk7PRaHSCd20WP5VpDJ4pfpzEhPt2u/0Gfpm5ABBMPpPbFWPrFninz+gpGQOwALxn/2xUkly48gxPruaBs/LTszANPuDk6Yv3HhAPBgNR29v4bxALgjNBrrwwBnAI2aR73wW8B0C6DrDpdZyuynmbE9ale+8BAQBdl3GN/y9Y9uDUCKnobwAAAABJRU5ErkJggg==")
            },
            {
                Icon.Shaders,
                new Lazy<string>(() =>
                    "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAADbElEQVR4Ae1bvYsTQRTfL0jYIh7XWAmCjSAelqLigWJ1XKOghYhVKsEiBEQEC+EQwerAP0E9LIUDQRAURNOJKQRBuFYsAgkm5GOzvheSc2YTZ94ys7sxMwND5uO9N/N++97bmZ2J22w2HVnqdrtOFEXOeDyWkQr7kb9cLju+7y+k6/V6C9upjXEcO57nOa7rTsYIw/CQtVqtHpbZgsdWTCxbAEx86qzO1gJYNEwsWwsw8amzOlsLYNEwsWwtwMSnzuocsBVB+QT07UC+CBkX2DHktGlmbbeA8XVa5qzoKQAc7/f7X2ECIW40VBJuUkDWS9ikrIOcvoosXbxBu92WydoFAlQ+khHK+gEAF3aEIYDwIgiCa+yuEHecCFDeycMtpCRvqj55RqkYZMWj0egqtG0z7YUVKTZNoUmjwHjqCnvAVErDmAUtRbmR7oHRE8DqJq6AH0mGw6HuIcjyKACQhaUgXBpXoLwFUuiVihRdwR8MBnsQDE8D569U3IuJMYridzWySRUJAEb9CF0BXOA7lDuLdaK3ggwX5HXh5wNwPYD8Q8ZdKADTyUUwYfxKuiabLLH/CLxlbkDehvXGKeA5EPEVFQOSc8I1hrY8XbOEnU5nF7Lzry/COIllASAJiHId1y7whtms1WpCWYW7QL1eX3xIIJw2ubMClMl9C7fcXFkLoEJkAaAitap0uccAmc8/27qiFes7+2+T8riYYF0gCY9pdeMtIPcYkLSw54/Oc003H875LNeftjIv/yMnwngLsABw9mBgpfAYsHbytxD2/VdnhP3yTrF86wJyBFebwngLKDwG+MfEdw+3rn9RMsE3nzaE/MZbgAVAaB8GdBYeA273/t7nRbyPvjurBPvPS585/qR8rhMq1gWSiJhWp1hA4W6S5UOhKCd+USvO7lv5Aidh/dxTrq5amZPv8DHCg3O5ydUUwe97PMNf1RRUKnh4Ikx3W63WZbzbo/GqjHDAPDsDvJwkSQelUmkD/jKzAyCoXJObDaPrFHgmT+nXbTQaJAE6/jMEZwI4Fvddfm7wx0/mmpQa7t8TslPeAkIB1M6p8lTy3OhyAWBZlUeUMwdgmZVHACjrAKTTmbjzeRDMxwSJz2qYCDd+5hagYcKZirAAZArvfyC8iBiQhIXzyWRn1nXrAlkjvOzyjbeAP1Ov811mOHZ+AAAAAElFTkSuQmCC")
            },
            {
                Icon.Terrains,
                new Lazy<string>(() =>
                    "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAADGklEQVR4Ae1bP6/TMBBP0kitMhTEJ3gSI4KJmTcwPwYYQEJ8BaZuzO8L8B147818A5iZ6IaE1J2hUiu16p80/K60eoll9WzsxGlsS1Fi++7s+/nuHNtJPB6PIy4tFosoz/Not9txpCfriX8wGES9Xk9Kt1wupeWqhUVRREmSRHEc79vIsoxlTViKjhMEADo+wKx6wQJYiDpOECyg4wPMqhcsgIWo4wTBAjo+wKx6KUvxj+Axbte4XuCiF+wCl246WtsHMH7VZa6LXgWAi9Vq9RMdyGihYZJokQJZt1ikPIKclYksW7zpbDbjZH0GASmfc4RcPQCIsSLMAMJNmqZvyqtCWnESQE2nhJaQzHVpOvIlpQrIKrbb7WuUXZXKnT2q2LQKjY4Cu4Mr3IGpr8NYB62KclvbDZMnwOr2rkCbJJvNxnYTyvJUAFAWpkHYGldQmQU09NIiJVfordfrOwTDp+D8o8UtJ6YoSvtqyiblEgCK+jm5AlzgF57ncp3USyEjhrwFbt/B9QnXb47bKQCHzuXoMO2SPuQ6q1j/ALPMW1xXeN94Ap7JKb42AED9M37HKCtJ0za9b8znc3qHeVWuE5/bAoDYL+P8AYRLTlDrABiNRvJDA04Tef1QXnxf6moavO+B46cAgOMBcN688xjA+fzky/MKSBfvf1TyXD2Ixb2LypIzuEAFTg8z3ltA4zGgbp/XjQneW0AAwMO4V1G58RhQaR0Zzmfrrg8uII6Ib3nvLcB5DBAtTvR52/WiPO8tIAAgmoRv+WABvo24qG+wABERSb51U6Wkj/9dpKKc2U8CTNfEPT6G3Hp1gnO5/acpJ+7f6Ay/qykdDtnDk4/T6fQlnbVZ/FSmNXim9HESkyb9fv8Zfpm5Bggmn8kdm7F1CnyUZ3RXiQHUAJ2zvzNqCczYDyQRlX167t3ftE2RX4w5jU2DB+XF/jjPNwJAW5Un9GsHoM3KEwCqMYBotZOK8qJPajdiyFCbBagob9h3K+y1AHAuyhOC1gE4J+UJgFpjADUgSZXzeUl9o0XWLaDR3ltoLABgAcSzFvEX9GrjTlBDadsAAAAASUVORK5CYII=")
            },
            {
                Icon.Textures,
                new Lazy<string>(() =>
                    "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAADKElEQVR4Ae1bva7TMBRO0kitMhTEC4DEiOARuAPzlRAMsDB2YWCoujHfF7gDjwB3RLwBbF3phoR0mRkqtVKr/qS536naK9+j1HYaO8mtbcmKnfNjn8/Hf7ETjkajQBVms1mQpmmw2WxUrFI6yXc6naDVauXyzefz3Pe6L7MsC6IoCsIw3JaRJMmtaK/Xu02LiUjMuJj2ALjY6qLN3gNENFxMew9wsdVFm70HiGi4mPYe4GKrizbHYkaSfgraBeJLRFpgZ4hFw97bPkDwR1FhW/w6ADxZLBa/UYGENhplAm1SoOsbNimPoGdRRpcp2Xgymah0XYKBjE9VjCo6AAixI0wAwtc4jt+Ku0LacRJAVYeItpCKeFa25QWjMujK1uv1G7w7F97XltTxaR2eIgZsdl3hCkLtIoI2eHWMW5sumHoCvG7bFegjyWq1Ml2Etj4dALSVFWBsTFfQmQUK2FWIlbpCa7lcXmEwfA7J/4Wk85lpFKXvatouVScANOqn1BXQBf4gPc23Sf8tdITQN8PjF6Q+I/5VSdcKwK5yKSpMX0kfqiqrSX+AWeYd4jnWG88gcy2Tq2sM4HWiNYaxuFuzJNPp9BIxOPRFmCrRFAA4IKXztHbBDHPW7/eluirvAoPBIP9QQFrNo4ldSEr3LSfrAbqQeQB0kTpVvsrHgKJAfv/4Tyry+stjKZ3Lc37fBaTwOUD0HuBAI0tN9B4ghccBovMeUPs6gM/T3On4vM3pZeWd9wAPAHcp1/LeA1xrcW6v9wCOiGv52tcBtud5vk7g5fku4JrLc3t1PKD2bsIrbTKvA0C5nwRM1taCrgjncturKZLnTzrDP9UQd7t0eCINn8bj8Su622Pwqoy0wCqJMV1OUoTrdrv9Ar/MXACEMtfk9sWYOgXe6yv11B3g6Jz9famSIIxzQVJx56yOz9O8DD5vc7pKnvPzvM4gyGWOyu+MP0rWplAlADTVeALWOgBNNp4ACIfDIT2V4Zj/Bg8Yf2cMUBZsmcGaBxww3rI5xdVbAeC+GE9wGQfgPhlPAOiuA4jXVKj+Srik5sY9QFJWI0kegEY2S4WVugEE8PIgK5HkygAAAABJRU5ErkJggg==")
            },
            {
                Icon.Android,
                new Lazy<string>(() =>
                    "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAADHElEQVR4Ae1bO4/TQBC2HUuJXARER4d0JTok+AFcgUR3FFBAQ3U9VTrq66joaeFqSjr4ASCRDumk6ygoghIpIQ/HfJNLdN7B8uzK8a1Zr6WVvTuP3fl2Zh9eOxwOh4F0TafTIE3TYL1eS6yldJLv9XpBp9Mp5JvNZoXluoVZlgVRFAVhGG7qSJJEFI1EDscZPACOd7BonvcAESLHGbwHON7BonneA0SIHGfwHuB4B4vmxSLHJcMBbqdID5FogZ0hmV47b3sJwY+mwnXx6wBwZz6ff0cDEtpoVLlokwJdH7BJuQU98yq69iUbj8djSddbMJDxqcQo0QFAiB1hAhDex3H8LL8rpB0nAXTdV0RbSCEdVe35nFEZdGWr1eopyo5z5dYedXxah8fEgPU2FM4g1DURrINXx7jVviumSIDXbUKBXpIsl8t9V6GtTwcAbWUGjI0JBZ1ZwMAuI1YKhc5isTjDYHgIyV9G0sXMNIrSezVtl7IJAI36KYUCQuAHnifFNumXQkcIfVPcvkDqNdK5JG0VgG3jUjSY3pLelBqrSb+BWeY50jHWG3chc1Em1wQAqH2V1xh5I2napvXGZDKhNcyTPI0/NwUA3q7K+S0IR5KixgEwGAyKDw0kS4rp/eLiq1Jb0+BVCyw/eQAsd4D16q2PAVLMn7wxmx3fDX5zUPm7C2XL6UOAw9W2fOs94NrHACnmbx+oTSqI6VIn5fI/z//ZzStjQus9wANQ6k8tIKoBZ8FgPs/zmOd0qYmSPKf7EJAQdZ3uPcD1Hpbs8x4gIeQ63XuA6z0s2ec9QELIdbqOB1hfLtfZCTrGVftJQGg9X5tz9m+f/ihFX1n+weOeQucZSX+Ec7nNpykl9890hu/qFff74uHJq9Fo9IjO2vb4qUxj8Izp4yThuuh2u/fwy8wpQKjymdyuGrP33Dupmu46YwBVTefsL2pqg/KOjtdxn8U4HwMkOteHvD8XyIOiMw3m+Z17bj0AumNAnT2vxCQqUsYEaR6X6NDH9Su2tN4DPACKP7Qw04QxgMNeGrOcuWreh0BVBP93+dZ7wF/AUNh5yHJbEgAAAABJRU5ErkJggg==")
            },
            {
                Icon.IPhone,
                new Lazy<string>(() =>
                    "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAADKUlEQVR4Ae1bP4/TMBRP0kitMhTEJ0BiRDAxcwPzLTDAgpi6MlTdmG9luG/AAjfzDWCrxEQ3JKTbGSq1Uqv+SXO/V5WT7ymxn0nShNqWrPjPe89+P7/n2LETTiaTwBQWi0WQpmmw2+1MpNp64u/1ekGn08mlWy6XueXSwizLgiiKgjAM920kSXLLOhgMbtNqIlIzLqY9AC6OuqqztwAVDRfT3gJcHHVVZ28BKhoupr0FuDjqqs6xmtGkH6HuAvE5Ii2wM0Tb8Nfa3oLxqy1zXfQSAB6uVquf6EBCG40ygTYpkPUFm5QHkLMqI6sq3ng2m5lkXYKAlE9NhKZ6ABBiR5gAhM9xHL9Sd4W04ySAjh0i2kIa4lnZkVeUyiAr2263L1F2rpQ3lpTYtITGRoHdwRWuwNS1YayDVqLctuqGyRNgdXtXoI8km82m6ibE8iQAiIVZELbGFSRvAQu9rEjJFTrr9foKk+ETcP6x4s4nplmUvquJTapJAGjWT8kV4AK/kJ7n6yQvhYwQ8hZ4fAfXB8TfJu5GATh0LkWH6SvpfVNnhfX38JZ5jXiO9cZj8Fzr+JqaA3ifaI1RWTysWZL5fH6JGBR9EaZOtAUADkjpPK1d8IY5Gw6HWlmNu8BoNMo/JNB2W1zZByXft9xZbp6sBUgh8gBIkTpVusbnAA7ss+EnXlQq/+PjOy2/dwEtPA5UOm8BrZsDuNGZfJjT284hzluAB4CbkGt5bwGujTjX11sAR8S1vLcA10ac6+stgCPiWr71ewE+IHytb7tX4PK8C3BEXMtLLOC/cxObQZQoV+4nAZve5NCafJ7X54jQFkU4l9tfTdE8v9EZ/qmGuN+nwxNteD+dTl/Q3Z4Kr8poGzxmZUyXkwzhutvtPsUvMxcAocw1uaJmqjoVLpKvLZfMASSAztnfaCX9QyXOBYmLn91pJZX1eS5c8hbgPJXkD8pXIquMkEYAaIvyBNzRAWiT8gRAOB6P6WkMVfw3WKC81Rxg7KiZoJn7AQXKm7tbM8VRXKCtyhO2tQPQZuUJAOk6gGjrCnd8sq5GiuTWbgFFDbel3APQlpFoqh832L7osvQPXYMAAAAASUVORK5CYII=")
            },
            {
                Icon.MacOS,
                new Lazy<string>(() =>
                    "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAADOUlEQVR4Ae1bP4/TMBRP0kitMhTEJ0BiRNzCgFi4gfkYYGBiQ4w3dWO+L3DjiRlu5huAKqEOLHRDQrqdoVIrteqfNLxXWl38FOXZsX1OE1uyEtvvPfv9/J7/JuF4PA64MJ/PgzRNg+12y5GWliN/r9cLOp1OId1isSjMl83MsiyIoigIw3BXR5IkLGvEUjScwAPQ8A5m1fMWwELUcAJvAQ3vYFY9bwEsRA0n8BbQ8A5m1YtZiv8Ej+BxAfEFRFxgZxBVw8Ha3gHjV1VmW/QyADxcLpe/oAEJbjR0Am5SQNYX2KQ8ADlLHVmmeOPpdMrJugQCVD7lCLlyACCEHWECIHyO4/hNfleIO04E6K5DhFtIJp7q9nxOqQxkZZvN5jXkneXynb3K2LQMjYoC270rXANTV4XRBq2MchvTFaMngNXtXAEPSdbrtekqpOXJACAtTIGwNq4gMwso6KVEiq7QWa1W1zAYPgHOv0rcxcQ4iuK5mrRJuQQAR/0UXQFc4De8z4p1ks8FGSHIm8PjO3B9hPiH43YKwL5xKTQYT0nvc42VLL8Hs8xbiGew3ngMPDdlfHUAANunvcbIK4nTNq43ZrMZrmFe5cvoe10AoO3STu9BOOUEHR0Ag8Gg+FKhWNN+cfZtrqtp8LYFjt88AI47wHn1tR8DOJ8///BDAPHy6rmQ5hLeBTiEml7eegtwPgZwPk4tsILP0/NL4dip9RbgAaAm1rZ0OBwOOZ0nQIBbVSM7Ns7nK/i40H5Vfu8CAnwtTLTeApyvA6jPUiPkyik93Qs8e/peIBn9/CSkW28BHgDBHlqYcD4G6GJOfZ7Koz5Py70LUETalpaxgKN3k7JOlVFO7yeBstoNlNF1Ah0T2HUA3MvtPk0peX7DO/ymhrjfZy9PzieTyUu8azP4qUxt8Izx4yQm3HS73RP4ZeYCQND5TO5Qjalb4IM8rWc4Go2kBJj4ZwjOArAuekYn1E99WiiskKBjAhUhMwtQnkrpvfKVeG0y3QkAdVUegbUOQJ2VRwBk1gFIZzII5/IgWBgTOJ810BChfusWYKDBVkV4AKzCewTCXYwBFBbBJ2mh7bR3AdsI111+6y3gHxXQ5njbLBGnAAAAAElFTkSuQmCC")
            },
            {
                Icon.WebGL,
                new Lazy<string>(() =>
                    "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAADJ0lEQVR4Ae1bvW7bMBCWZAE2NDhFn6BAxyJ9hBpo5yzt0Kmj1w6Gt855gTxEm7kPUCDdvNZbgADZMxiwARv+kdU7ww7og0oeLVJUTRIgRIp3R97HuxMpSvF4PI5UaT6fR3meR9vtVkUqbUf+TqcTtVqtUrrFYlF6n3uzKIooSZIojuNdH1mWPbP2+/3nslhIxIqP5QCAj7Mu6hwsQETDx3KwAB9nXdQ5WICIho/lYAE+zrqocypWJOXX0HYN+R1kXGAXkHXTwdq+AONPXWZb9BwAXi2Xyz8wgAw3GlUSblJA1g/YpLwEOcsqskzxptPpVCXrBghQ+VxFqGoHAGLYEWYAwvc0TT+Ju0LccSJAdacEt5CK3Ks684JSBcgqNpvNR7h3Jdx3VuTYNIdGR4Ht3hVugamtw2iDlqPcxnTH6AlgdTtXwJck6/XadBdseRwA2MI0CBvjCpyngIZeWqToCq3VanULwfASOJ+0uMuJMYriezW2SbkEAKN+jq4ALnAP5Vm5Tvy7ICMGeXO4/Aaub5AfVNxOAdgPLocB41vSF6rBMtsv4CnzGfIVrDfeAM+jjM9VDKBjwjWGsbxfs2Sz2ewGcvSvN8I4iKYAQAGpXMe1CzxheoPBQCqrdhcYDoflhwLSYZ7c2AVO6b7lbC2AC1kAgIvUudLVHgMokHfv690V934dbz+CC9AZ8a3uvQU4jwEqi6M+q6LXjSneW0AAQGVS597uPAZQH1f5sKqdThiVT9uDC1BEfKt7bwHOY4Bpn6byaJ3GBO8tIADgW9Cj+jqPAdQnqc/SOlWA8uu2BxegiPlW51iAczexOSkc5ar9JKAYfVUfV4iPqHwaMxI4l9t9miK53uEZ/rmmtNvFwxNp+jqZTD7gtz0GP5WRdlhnY4ofJynSY7vdfgu/zFwDCFU+kzt0Y+oU+CCv0jUejUYsASb+GYJzQexLelZHfZY1OAkR9XlKynkKUJ6T6nvlT+K1yVQLAE1VHoG1DkCTlUcAOOsApDOZ6OegRzFB5bMGBnLUv3ULMDBgqyICAFbh/Q+Eu4gBFJYjn6SNtuvBBWwj3HT53lvAX07q+IeImmfxAAAAAElFTkSuQmCC")
            },
            {
                Icon.Windows,
                new Lazy<string>(() =>
                    "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAADEklEQVR4Ae1bvW7bMBCWZAE2NLhFp44FOhbtIyRD5yzN0C4dvXYwPLVz+gB5iDZLlrxBunmttwIFMhboYMAGbPhHVr4z7IASZPIYypIikgAh/twdeR/vKFKk/NFo5KnCbDbz4jj2NpuNilRaT/ydTsdrtVq5dPP5PLecW5gkiRcEgef7/raNKIoeWHu93kNaTARixsa0A8DGURd1dhYgomFj2lmAjaMu6uwsQETDxrSzABtHXdQ5FDOS9GvUXSCeINICO0HUDXtr+wzGG13mY9FzAHi1WCx+owMRbTRMAm1SIOsnNikvIGdhIqso3nAymahkXYKAlI9VhKp6AOBjRxgBhB9hGJ6Lu0LacRJAZYeAtpCKeGo68oJSCWQl6/X6A8rOhPLKkhyb5tDoKLDZucIVmNo6jMeg5Si3Lrph8gRY3dYV6CPJarUqugm2PA4AbGEahLVxBc5bQEMvLVJyhdZyubzCZPgWnP+1uPOJaRal72psk6oSAJr1Y3IFuMAfpKf5OvFLIcOHvBkev8D1DfGvirtSAHadi9Fh+kr6XNVZZv0zvGU+Ip5hvfEGPHcyvqrmgGyfaI1RWNytWaLpdHqJ6B36IkydqAsAWUCM87R2wRvmtN/vS2WV7gKDwSD/UEDazUdXdsEp3bc01gK4kDkAuEg1la70OSAL5Mvv19kiaf7f1/NUvSm/c4EUnBZmnAVYOOgplZ0FpOCwMOMswMJBT6nsLCAFh4UZ6y2g8r1Adm2va4Sm/NZbgANA1+SaRl/5HGC6nzfldy7QNJPW1YdjAZW7ia5SOvQcAMx+EtDpTQW0Ac7ltldTJM9bOsNvagi7XTo8kYYv4/H4Pd3tKfCqjLTBMitDupykCHftdvsdfpm5AAgm1+T2zRR1CryXZ/TkTnB0zv7JqCUw41yQREjP6kzb0OXnTIK6MnPpd8rn1lVZWAoAdVWegD86AHVWngDwh8MhPZXhMf8NHlDejjnggPJKoMsmOIoLPBXlCezCAXhKyhMA3HUA0RYVyr8SLul54RYgaauWVQ6AWg5LiZ26B0AX6YrQwLv9AAAAAElFTkSuQmCC")
            },
        };

        private static readonly Dictionary<Tree, Texture2D> TreeTextures = new Dictionary<Tree, Texture2D>();

        private static readonly Dictionary<Tree, Lazy<string>> TreeStrings = new Dictionary<Tree, Lazy<string>>
        {
            {
                Tree.Normal,
                new Lazy<string>(() =>
                    "iVBORw0KGgoAAAANSUhEUgAAAgAAAABACAYAAABsv8+/AAAETUlEQVR4Ae3dwa0aQRAEUK9FFE4D0nAUI0EapAFpOA1Iw3FYvtaBw/SpxPu3Fmqp+81fbWkvc6y1fvgjQIAAAQIEvkvg53eta1sCBAgQIEDgv4AA4P+AAAECBAh8oYAA8IWHbmUCBAgQIHAqIPgVM/6Nur08xwLvqNvL37HAn6jby2ss8Iy6vbzHAlnHz3XlIya+Rd1e5vOWz2P7fq9Y4BJ1e5nvu3wfjvbzBWDEp5kAAQIECHQKCACd52ZqAgQIECAwEhAARnyaCRAgQIBAp4AA0HlupiZAgAABAiMBAWDEp5kAAQIECHQKCACd52ZqAgQIECAwEhAARnyaCRAgQIBAp8DhLoDOgzM1AQIECBCYCPgCMNHTS4AAAQIESgUEgNKDMzYBAgQIEJgICAATPb0ECBAgQKBUQAAoPThjEyBAgACBiYAAMNHTS4AAAQIESgUEgNKDMzYBAgQIEJgICAATPb0ECBAgQKBU4FQwd95/nPcjF6zwccRz/PqOur3M+8fzfvL2/a6xwDPq9vIeC2QdP9eVj5j4FnV7mc9bPo/t+71igUvU7WW+7/J9ONrPF4ARn2YCBAgQINApIAB0npupCRAgQIDASEAAGPFpJkCAAAECnQICQOe5mZoAAQIECIwEBIARn2YCBAgQINApIAB0npupCRAgQIDASEAAGPFpJkCAAAECnQLHWqtzclMTIECAAAEC2wK+AGzTaSRAgAABAr0CAkDv2ZmcAAECBAhsCwgA23QaCRAgQIBAr4AA0Ht2JidAgAABAtsCAsA2nUYCBAgQINArIAD0np3JCRAgQIDAtoAAsE2nkQABAgQI9AqcCkbP+4/zfuSCFT6OeI5f31G3l3n/eN5P3r7fNRZ4Rt1e3mOBrOPnuvIRE9+ibi/zecvnsX2/Vyxwibq9zPddvg9H+/kCMOLTTIAAAQIEOgUEgM5zMzUBAgQIEBgJCAAjPs0ECBAgQKBTQADoPDdTEyBAgACBkYAAMOLTTIAAAQIEOgUEgM5zMzUBAgQIEBgJCAAjPs0ECBAgQKBT4FhrdU5uagIECBAgQGBbwBeAbTqNBAgQIECgV0AA6D07kxMgQIAAgW0BAWCbTiMBAgQIEOgVEAB6z87kBAgQIEBgW0AA2KbTSIAAAQIEegUEgN6zMzkBAgQIENgWEAC26TQSIECAAIFegVPB6Hn/cd6PXLDCxxHP8es76vYy7x/P+8nb97vGAs+o28t7LJB1/FxXPmLiW9TtZT5v+Ty27/eKBS5Rt5f5vsv34Wg/XwBGfJoJECBAgECngADQeW6mJkCAAAECIwEBYMSnmQABAgQIdAoIAJ3nZmoCBAgQIDASEABGfJoJECBAgECngADQeW6mJkCAAAECIwEBYMSnmQABAgQIdAoca63OyU1NgAABAgQIbAv4ArBNp5EAAQIECPQKCAC9Z2dyAgQIECCwLSAAbNNpJECAAAECvQL/AG/bKlGRkrW/AAAAAElFTkSuQmCC")
            },
            {
                Tree.Height,
                new Lazy<string>(() =>
                    "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAA8UlEQVR4Ae2ZQQrDMBADm9Kf+v9vaOnVh0BAgkx2cltihCyNffGx1npN/t6TN//fuwFIwPAEPALDAfAS/NyAgO/m4djm6ugdUI0XIC4BgJKqFiWgGi9AXAIAJVUtSkA1XoC4BABKqlqUgGq8AHEJAJRUtSgB1XgB4hIAKKlqUQKq8QLExxNwh3eBnZP9nWD/f3U+fWcYT4ABXOXpaevveAecntl0AR6BdKI0PQmgNZb2KwHpRGl6EkBrLO1XAtKJ0vQkgNZY2q8EpBOl6UkArbG0XwlIJ0rTkwBaY2m/EpBOlKYnAbTG0n4lIJ0oTW88AT8muwOwK+8yWQAAAABJRU5ErkJggg==")
            },
            {
                Tree.Middle,
                new Lazy<string>(() =>
                    "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAA8klEQVR4Ae2aMQ6EMADDAPHT/v8Nd2OlDgwo0Z1VM8GAiRLDxDnGOH58fJbnn8t19fKq0gFwCwCMVI2oAdV6AXANAIxUjagB1XoBcA0AjFSNqAHVegFwDQCMVI2oAdV6AXANAIxUjagB1XoBcA0AjFSNqAHVegFwDQCMVI2oAdV6AXANAIxUjXhX6e/g6/8C7yjzrsf/DXwFZlF7nm1vwD9+Ax7f2bSn2xtgAWmlaDwNoC2WzqsB6UZpPA2gLZbOqwHpRmk8DaAtls6rAelGaTwNoC2WzqsB6UZpPA2gLZbOqwHpRmk8DaAtls6rAelGabwvvrQDrtVOP8MAAAAASUVORK5CYII=")
            },
            {
                Tree.Bottom,
                new Lazy<string>(() =>
                    "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAA70lEQVR4Ae2aMQ5AQAAEET+9/7+BUnKFQjKxmxsVhbHZHSr7GGP7+bim5+/TNXp5oPQCuAUUjIRG1AC03gK4BhSMhEbUALTeArgGFIyERtQAtN4CuAYUjIRG1AC03gK4BhSMhEbUALTeArgGFIyERtQAtN4CuAYUjIRG1AC03gK4BhSMhEY8Ufo3+Py/wDfKc9fr/wa+Ak9Ra54tb0DCN+D1HaW9XN4AC6AVS+drQPpCdD4NoBtO52tA+kJ0Pg2gG07na0D6QnQ+DaAbTudrQPpCdD4NoBtO52tA+kJ0Pg2gG07na0D6QnQ+DaAbTuffvsQDriqTua4AAAAASUVORK5CYII=")
            },
        };

        public static readonly Dictionary<string, Icon> icons = new Dictionary<string, Icon>
        {
            { "Animations", Icon.Animations },
            { "Resources", Icon.Resources },
            { "Scenes", Icon.Scenes },
            { "Scripts", Icon.Scripts },
            { "Plugins", Icon.Plugins },
            { "Materials", Icon.Materials },
            { "Extensions", Icon.Editor },
            { "Audios", Icon.Audios },
            { "Prefabs", Icon.Prefabs },
            { "Models", Icon.Meshes },
            { "Settings", Icon.Project },
            { "Shaders", Icon.Shaders },
            { "Fonts", Icon.Fonts },
            { "Textures", Icon.Textures },
            { "StreamingAssets", Icon.Resources },
            { "Physics", Icon.Physics },
            { "Terrains", Icon.Terrains },
            { "Tilemaps", Icon.Terrains },
            { "Lights", Icon.Lights },
            { "Process", Icon.Lights },
            { "Editor", Icon.Editor },
            { "Android", Icon.Android },
            { "iOS", Icon.IPhone },
            { "Windows", Icon.Windows },
            { "MacOS", Icon.MacOS },
            { "WebGL", Icon.WebGL },
            { "DataTable", Icon.Project },
            { "Atlas", Icon.Meshes },
            { "Icons", Icon.Textures },
            { "HotUpdate", Icon.Scripts },
            { "Template", Icon.Resources },
        };

        public static Texture2D GetIcon(Icon type)
        {
            return GetIcon(type, IconStrings, IconTextures);
        }

        public static Texture2D GetIcon(Tree type)
        {
            return GetIcon(type, TreeStrings, TreeTextures);
        }

        private static Texture2D GetIcon<T>(T value, Dictionary<T, Lazy<string>> strings, Dictionary<T, Texture2D> textures) where T : Enum
        {
            if (textures.TryGetValue(value, out var texture))
            {
                return texture;
            }

            if (!strings.TryGetValue(value, out var result))
            {
                texture = Texture2D.grayTexture;
                textures.Add(value, texture);
                return texture;
            }

            texture = new Texture2D(4, 4, TextureFormat.DXT5, false)
            {
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Bilinear,
                hideFlags = HideFlags.HideAndDontSave
            };
            texture.LoadImage(Convert.FromBase64String(result.Value));
            textures.Add(value, texture);
            return texture;
        }
    }

    internal enum Icon
    {
        Animations,
        Audios,
        Editor,
        Lights,
        Fonts,
        Materials,
        Meshes,
        Physics,
        Plugins,
        Prefabs,
        Project,
        Resources,
        Scenes,
        Scripts,
        Shaders,
        Terrains,
        Textures,
        Android,
        IPhone,
        MacOS,
        WebGL,
        Windows,
    }

    internal enum Tree
    {
        Normal,
        Bottom,
        Middle,
        Height,
    }
}