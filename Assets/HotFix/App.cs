using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HotFix
{
    struct MyValue
    {
        public int x;
        public float y;
        public string s;
    }
    public class App
    {
        public static int Main()
        {
#if !UNITY_EDITOR
 Debug.LogError("UNITY_EDITOR");
            LoadMetadataForAOTAssembly();
            Debug.Log("ydd-- AOT���򼯼������!");
#endif
            // ���Բ���Ԫ���ݺ�ʹ�� AOT����
            TestAOTGeneric();

            LoadScene();
            return 0;
        }

        /// <summary>
        /// �л�����
        /// </summary>
        static async void LoadScene()
        {
            var handler = await Addressables.LoadSceneAsync("MainScene").Task;
            handler.ActivateAsync();
        }


        /// <summary>
        /// ���� aot����
        /// </summary>
        public static void TestAOTGeneric()
        {
            var arr = new List<MyValue>();
            arr.Add(new MyValue() { x = 1, y = 10, s = "abc" });
            Debug.Log("AOT���Ͳ���Ԫ���ݻ��Ʋ�������");
        }

        /// <summary>
        /// Ϊaot assembly����ԭʼmetadata�� ��������aot�����ȸ��¶��С�
        /// һ�����غ����AOT���ͺ�����Ӧnativeʵ�ֲ����ڣ����Զ��滻Ϊ����ģʽִ��
        /// </summary>
        public static unsafe void LoadMetadataForAOTAssembly()
        {
            // ���Լ�������aot assembly�Ķ�Ӧ��dll����Ҫ��dll������unity build���������ɵĲü����dllһ�£�������ֱ��ʹ��ԭʼdll��
            // ������Huatuo_BuildProcessor_xxx������˴�����룬��Щ�ü����dll�ڴ��ʱ�Զ������Ƶ� {��ĿĿ¼}/HuatuoData/AssembliesPostIl2CppStrip/{Target} Ŀ¼��

            /// ע�⣬����Ԫ�����Ǹ�AOT dll����Ԫ���ݣ������Ǹ��ȸ���dll����Ԫ���ݡ�
            /// �ȸ���dll��ȱԪ���ݣ�����Ҫ���䣬�������LoadMetadataForAOTAssembly�᷵�ش���

            foreach (var dllBytes in LoadDll.aotDllBytes)
            {
                fixed (byte* ptr = dllBytes.bytes)
                {
                    // ����assembly��Ӧ��dll�����Զ�Ϊ��hook��һ��aot���ͺ�����native���������ڣ��ý������汾����
                    int err = HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly((IntPtr)ptr, dllBytes.bytes.Length);
                    Debug.Log($"LoadMetadataForAOTAssembly:{dllBytes.name}. ret:{err}");
                }
            }
        }
    }
}
