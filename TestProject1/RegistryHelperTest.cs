using RegistryClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for RegistryHelperTest and is intended
    ///to contain all RegistryHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RegistryHelperTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        public string key = @"Software\SIMAC\Test_new";
        public string reg_sZ = "teststring12234";
        public long dword = 6514654165416;
        public long overwrite_dword = 4294967295;
        public ulong qword = 18446744073709551615;
        public string[] multiArray = new string[] { "string1", "string2", "string3", "string4" };
        public string multiContent = "weq4tvwcw54wxrcsfvv3b4z6gebrh45v645b6";
        public int multiCount = 10000;
        public string expandTestString = "exPTestString";

        public RegistryClass.RegistryHelper.ROOT_KEY root = RegistryHelper.ROOT_KEY.HKEY_CURRENT_USER;
        public RegistryHelper oReg = new RegistryHelper();

        /// <summary>
        ///A test for WriteString
        ///</summary>
        [TestMethod()]
        public void WriteStringTest()
        {
            bool result = oReg.WriteValue(root, key, RegistryHelper.VALUE_TYPE.REG_SZ, "reg_sz", reg_sZ);
            Assert.AreEqual(result, true, "Write REG_SZ");
            Assert.AreEqual((string)oReg.ReadValue(root, key, "reg_sz").Value, reg_sZ, "Read reg_sz");
        }

        /// <summary>
        ///A test for WriteQword
        ///</summary>
        [TestMethod()]
        public void WriteQwordTest()
        {
            bool resultQW = oReg.WriteValue(root, key, RegistryHelper.VALUE_TYPE.REG_QWORD, "QW", qword);
            Assert.AreEqual(resultQW, true, "Write QWORD");
            resultQW = oReg.WriteValue(root, key, RegistryHelper.VALUE_TYPE.REG_QWORD, "QWminus", -1844674407370955161);
            Assert.AreEqual(resultQW, false, "Write minus QWORD");
            Assert.AreEqual((ulong)oReg.ReadValue(root, key, "QW").Value, qword, "Read QWORD");
            Assert.AreEqual(oReg.ReadValue(root, key, "QWnone").Value, null, "Read none existing QWORD");
        }

        /// <summary>
        ///A test for WriteMulti
        ///</summary>
        [TestMethod()]
        public void WriteMultiTest()
        {
            bool resultMulti = oReg.WriteValue(root, key, RegistryHelper.VALUE_TYPE.REG_MULTI_SZ, "multi", multiArray);
            Assert.AreEqual(resultMulti, true, "Write Multi");
            Assert.AreEqual(((string[])oReg.ReadValue(root, key, "multi").Value)[0], multiArray[0], "Read Multi");
            Assert.AreEqual(((string[])oReg.ReadValue(root, key, "multi").Value).Length, 4, "Count Multi");
            Assert.AreEqual((string)oReg.ReadValue(root, key, "noneexist").Value, null, "Read None");

            oReg = new RegistryHelper();
            List<string> test = new List<string>();
            for (int i = 0; i < multiCount; i++)
            {
                test.Add(multiContent);
            }
            bool t = oReg.WriteValue(root, key, RegistryHelper.VALUE_TYPE.REG_MULTI_SZ, "Test_Multi_SZ", test.ToArray());
            Assert.AreEqual(t, true, "write " + multiCount);
            RegObject or = oReg.ReadValue(root, key, "Test_Multi_SZ");
            Assert.AreEqual(((string[])or.Value)[multiCount - 1], new string[] { multiContent }[0], "read " + multiCount + " type");
            Assert.AreEqual(((string[])or.Value).Length, multiCount, "read " + multiCount + " length");
        }

        /// <summary>
        ///A test for WriteExpand
        ///</summary>
        [TestMethod()]
        public void WriteExpandTest()
        {
            bool ex = oReg.WriteValue(root, key, RegistryHelper.VALUE_TYPE.REG_EXPAND_SZ, "eExpandTest", expandTestString);
            Assert.AreEqual(ex, true, "Write Expand");
            Assert.AreEqual((string)oReg.ReadValue(root, key, "eExpandTest").Value, expandTestString, "Read Expand");
        }

        /// <summary>
        ///A test for WriteDword
        ///</summary>
        [TestMethod()]
        public void WriteDwordTest()
        {
            bool resultDW = oReg.WriteValue(root, key, RegistryHelper.VALUE_TYPE.REG_DWORD, "DW", dword);
            Assert.AreEqual(resultDW, false, "Write DWORD");
            resultDW = oReg.WriteValue(root, key, RegistryHelper.VALUE_TYPE.REG_DWORD, "DW", overwrite_dword);
            Assert.AreEqual(resultDW, true, "over write DWORD");
            RegObject reg = oReg.ReadValue(root, key, "DW");
            Assert.AreEqual((uint)oReg.ReadValue(root, key, "DW").Value, overwrite_dword, "Read DOWRD");
            Assert.AreEqual(oReg.ReadValue(root, key, "DW2346z65").Value, null, "Read none exist DOWRD");
        }

        /// <summary>
        ///A test for WriteBinary
        ///</summary>
        [TestMethod()]
        public void WriteBinaryTest()
        {
            string stringByte = "test byte array";
            byte[] binaryArray = oReg.StringToByteArray(stringByte);

            bool resultBin = oReg.WriteValue(root, key, RegistryHelper.VALUE_TYPE.REG_BINARY, "bin", binaryArray);
            Assert.AreEqual(resultBin, true, "Write Binary");
            Assert.AreEqual(oReg.ByteArrayToString((byte[])oReg.ReadValue(root, key, "bin").Value), stringByte, "Read Binary");

            Assert.IsTrue(oReg.WriteValue(root, key, RegistryHelper.VALUE_TYPE.REG_BINARY, "wer", new byte[] { }), "write null binary");
            RegObject tBin = oReg.ReadValue(root, key, "wer");
            Assert.AreEqual(((byte[])tBin.Value).Length, 0, "read null Binary length");
            Assert.AreEqual(oReg.ByteArrayToString((byte[])tBin.Value), "", "Read empty Binary");
        }

        /// <summary>
        ///A test for ValueExists
        ///</summary>
        [TestMethod()]
        public void ValueExistsTest()
        {
            bool ex = oReg.ValueExists(root, key, "eExpandTest");
            Assert.AreEqual(ex, true, "Value eExpandTest exists");
        }

        /// <summary>
        ///A test for Regconnect
        ///</summary>
        [TestMethod()]
        public void RegconnectTest()
        {
            RegistryHelper oRegOld = oReg;
            Assert.IsNotNull(oReg.Regconnect("localhost", RegistryHelper.ROOT_KEY.HKEY_LOCAL_MACHINE));
            Assert.AreSame(oReg, oRegOld, " connect on `localmachine`");
            Assert.IsNotNull(oReg.Regconnect(".", RegistryHelper.ROOT_KEY.HKEY_LOCAL_MACHINE));
            Assert.AreSame(oReg, oRegOld, " connect on `.`");
            Assert.IsNotNull(oReg.Regconnect("", RegistryHelper.ROOT_KEY.HKEY_LOCAL_MACHINE));
            Assert.AreSame(oReg, oRegOld, " connect on ``");
            Assert.IsNotNull(oReg.Regconnect("NOTEXISTSMACHIONENAME", RegistryHelper.ROOT_KEY.HKEY_LOCAL_MACHINE));
            Assert.AreSame(oReg, oRegOld," connect on `NOTEXISTSMACHIONENAME`");
        }


        /// <summary>
        ///A test for KeyExists
        ///</summary>
        [TestMethod()]
        public void KeyExistsTest()
        {
            Assert.IsTrue(oReg.KeyExists(root, key), " TEST exists key");
            Assert.IsFalse(oReg.KeyExists(root, key+ " non exists 123"), " TEST Nonexists key");
        }

        /// <summary>
        ///A test for AccessTest
        ///</summary>
        [TestMethod()]
        public void AccessTestTest()
        {
            string sLocalMashinekey = @"SYSTEM\CurrentControlSet";
            Assert.IsTrue(oReg.AccessTest(root,sLocalMashinekey)," test Mashinekey");
            Assert.IsTrue(oReg.AccessTest(root, key),"test UserKey");                
        }


        /// <summary>
        ///A test for EnableAccess
        ///</summary>
        [TestMethod()]
        public void EnableAccessTest()
        {
            string sLocalMashinekey = @"SYSTEM\CurrentControlSet";
            Assert.IsTrue(oReg.EnableAccess(true));
            Assert.IsTrue(oReg.WriteValue(root, sLocalMashinekey, RegistryHelper.VALUE_TYPE.REG_SZ, "test", "none"));
            Assert.IsTrue(oReg.EnableAccess(false));
            Assert.IsTrue(oReg.WriteValue(root, sLocalMashinekey, RegistryHelper.VALUE_TYPE.REG_SZ, "test", "none"));
        }

        /// <summary>
        ///A test for EnumValues
        ///</summary>
        [TestMethod()]
        public void EnumValuesTest()
        {
            string SubTestKey = key + "\\subkey";
            string SubTestKey2 = SubTestKey + "\\subkey";
            string SubTestKey3 = SubTestKey2 + "\\subkey";

            for (int i = 0; i < 112; i++)
            {
                Assert.IsTrue(oReg.WriteValue(root, SubTestKey3, RegistryHelper.VALUE_TYPE.REG_SZ, "a" + i.ToString(), i.ToString()), "write Value : a" + i.ToString());
            }

            Assert.IsTrue(oReg.EnumValues(root, SubTestKey3).Count == 112);
        }

        /// <summary>
        ///A test for EnumKeys
        ///</summary>
        [TestMethod()]
        public void EnumKeysTest()
        {
            string SubTestKey = key + "\\subkey";
            oReg.DeleteKey(root, SubTestKey);

            for (int i = 0; i < 10; i++)
            {
                Assert.IsTrue(oReg.CreateKey(root, SubTestKey + @"\testkey" + i.ToString()), "create subkey :" + @"\testkey" + i.ToString());
            }

            Assert.IsTrue(oReg.EnumKeys(root, SubTestKey).Count == 10);

        }

        /// <summary>
        ///A test for DeleteValue
        ///</summary>
        [TestMethod()]
        public void DeleteValueTest()
        {
            string SubTestKey = key + "\\subkey";
            string SubTestKey2 = SubTestKey + "\\subkey";
            string SubTestKey3 = SubTestKey2 + "\\subkey";

            for (int i = 0; i < 10; i++)
            {
                Assert.IsTrue(oReg.WriteValue(root, SubTestKey3, RegistryHelper.VALUE_TYPE.REG_SZ, "a" + i.ToString(), i.ToString()), "write Value : a" + i.ToString());
            }

            for (int i = 0; i < 10; i++)
            {
                Assert.IsTrue(oReg.DeleteValue(root, SubTestKey3, "a" + i.ToString()), "delete Value : a" + i.ToString());
                Assert.IsFalse(oReg.ValueExists(root, SubTestKey3, "a" + i.ToString()), "test Value : a" + i.ToString());
            }
        }

        /// <summary>
        ///A test for DeleteKey
        ///</summary>
        [TestMethod()]
        public void DeleteKeyTest()
        {
            string SubTestKey = key + "\\subkey";
            string SubTestKey2 = SubTestKey + "\\subkey";
            string SubTestKey3 = SubTestKey2 + "\\subkey";

            for (int i = 0; i < 10; i++)
            {
                Assert.IsTrue(oReg.WriteValue(root, SubTestKey3, RegistryHelper.VALUE_TYPE.REG_SZ, "a" + i.ToString(), i.ToString()));
            }

            Assert.IsTrue(oReg.DeleteKey(root, SubTestKey), "delete key with subkeys");
            Assert.IsTrue(oReg.DeleteKey(root, SubTestKey2), "delete key without subkeys");

            for (int i = 0; i < 10; i++)
            {
                Assert.IsTrue(oReg.WriteValue(root, SubTestKey, RegistryHelper.VALUE_TYPE.REG_SZ, "a" + i.ToString(), i.ToString()));
            }

            Assert.IsTrue(oReg.DeleteKey(root, SubTestKey2), "delete key with subvalues");
        }

        /// <summary>
        ///A test for CreateKey
        ///</summary>
        [TestMethod()]
        public void CreateKeyTest()
        {
            string SubTestKey = key + "\\subkey";
            string SubTestKey2 = SubTestKey + "\\subkey";
            string SubTestKey3 = SubTestKey2 + "\\subkey";
            
            Assert.IsTrue(oReg.CreateKey(root,SubTestKey3),"create subkey 3");
            Assert.IsTrue(oReg.KeyExists(root, SubTestKey3),"is exists subkey 3");           
        } 
    }
}
