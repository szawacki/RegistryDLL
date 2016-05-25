using RegistryClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for RegObjectTest and is intended
    ///to contain all RegObjectTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RegObjectTest
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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            new TestProject1.RegObjectTest().createRgistryKeys();
        }
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

        public string key = @"Software\SIMAC\Test";
        public RegistryClass.RegistryHelper.ROOT_KEY root = RegistryHelper.ROOT_KEY.HKEY_CURRENT_USER;
        public RegistryHelper oReg = new RegistryHelper();


        public void createRgistryKeys()
        {
            if (oReg.KeyExists(root, key))
            {
                bool result = oReg.DeleteKey(root, key);
                Assert.AreEqual(result, true);
            }
            // dump registry
            oReg.WriteValue(root, key, RegistryHelper.VALUE_TYPE.REG_DWORD, "DW", "51664468");
            oReg.WriteValue(root, key, RegistryHelper.VALUE_TYPE.REG_QWORD, "QW", "36329457509");
            oReg.WriteValue(root, key, RegistryHelper.VALUE_TYPE.REG_BINARY, "Bin",  oReg.StringToByteArray("c-sharp ist scheisse"));
            oReg.WriteValue(root, key, RegistryHelper.VALUE_TYPE.REG_EXPAND_SZ, "exp", "123456öaslkfjösaodjfäüsadojgfäsdk");
            oReg.WriteValue(root, key, RegistryHelper.VALUE_TYPE.REG_MULTI_SZ, "multi", new string[] {"aaa", "bbb", "ccc"});
            oReg.WriteValue(root, key, RegistryHelper.VALUE_TYPE.REG_SZ, "REG_SZ", "string123456");
        }

        #region Test Constructor
        /// <summary>
        ///A test for RegObject Constructor
        ///</summary>
        [TestMethod()]
        public void RegObjectConstructorTest()
        {
            RegObject ro = new RegObject();
            Assert.IsNotNull(ro);
        }

        #endregion Test Constructor

        #region Test ToByteArray

        [TestMethod()]
        public void ReadBinary()
        {
            RegObject regBin = oReg.ReadValue(root, key, "bin");
            byte a = 99;
            byte z = 101;
            Assert.AreEqual(regBin.Value.GetType(), typeof(byte[]));
            Assert.AreEqual(((byte[])regBin.Value)[0], a);
            Assert.AreEqual(((byte[])regBin.Value)[19], z);
        }

        [TestMethod()]
        public void ReadBinarySize()
        {
            RegObject regBin = oReg.ReadValue(root, key, "bin");
            Assert.AreEqual(((byte[])regBin.Value).Length, 20);
        }

        [TestMethod()]
        public void ReadBinaryNone()
        {
            RegObject regBin = oReg.ReadValue(root, key, "bin2");
            Assert.AreEqual(((byte[])regBin.Value), null);
        }

        /// <summary>
        ///A test for ToByteArray
        ///</summary>
        [TestMethod()]
        public void ToByteArrayTestBin()
        {
            byte[] bin = oReg.ReadValue(root, key, "bin").ToByteArray();
            byte[] test = oReg.StringToByteArray("c-sharp ist scheisse");
            Assert.AreEqual(System.Convert.ToBase64String(bin),
               System.Convert.ToBase64String(test)); 
        }
        [TestMethod()]
        public void ToByteArrayTestMulti()
        {
            String a = System.Convert.ToBase64String(oReg.ReadValue(root, key, "multi").ToByteArray());
            String b = System.Convert.ToBase64String(oReg.StringToByteArray("aaa\0bbb\0ccc"));
            Assert.AreEqual(a,b );
        }
        [TestMethod()]
        public void ToByteArrayTestSZ()
        {
            String a = System.Convert.ToBase64String(oReg.ReadValue(root, key, "reg_sz").ToByteArray());
            String b = System.Convert.ToBase64String(oReg.StringToByteArray("string123456"));
            Assert.AreEqual(a, b);
        }

        [TestMethod()]
        public void ToByteArrayTestNone()
        {
            object a = oReg.ReadValue(root, key, "not exists").ToByteArray();
            object b = null;
            Assert.AreEqual(a, b);
        }
         
        [TestMethod()]
        public void ToByteArrayTestDWORD()
        {
            String a = System.Convert.ToBase64String(oReg.ReadValue(root, key, "dw").ToByteArray());
            String b = System.Convert.ToBase64String(oReg.StringToByteArray("51664468"));
            Assert.AreEqual(a, b);
        }

        [TestMethod()]
        public void ToByteArrayTestQWORD()
        {
            String a = System.Convert.ToBase64String(oReg.ReadValue(root, key, "qw").ToByteArray());
            String b = System.Convert.ToBase64String(oReg.StringToByteArray("36329457509"));
            Assert.AreEqual(a, b);
        }

        [TestMethod()]
        public void ToByteArrayTestExp()
        {           
            String a = System.Convert.ToBase64String(oReg.ReadValue(root, key, "exp").ToByteArray());
            String b = System.Convert.ToBase64String(oReg.StringToByteArray("123456öaslkfjösaodjfäüsadojgfäsdk"));
            Assert.AreEqual(a, b);
        }
        #endregion Test ToByteArray

        #region Bin
        /// <summary>
        ///A test for ToLong
        ///</summary>
        [TestMethod()]
        public void ToLongTest_bin()
        {
            Assert.AreEqual(oReg.ReadValue(root, key, "bin").ToLong(), -1);
        }

        /// <summary>
        ///A test for ToLong
        ///</summary>
        [TestMethod()]
        public void ToLongTest_multi()
        {
            Assert.AreEqual(oReg.ReadValue(root, key, "multi").ToLong(), -1);
        }

        /// <summary>
        ///A test for ToLong
        ///</summary>
        [TestMethod()]
        public void ToLongTest_regSZ()
        {
            Assert.AreEqual(oReg.ReadValue(root, key, "reg_sz").ToLong(), -1);
        }

        /// <summary>
        ///A test for ToLong
        ///</summary>
        [TestMethod()]
        public void ToLongTest_notexists()
        {
            Assert.AreEqual(oReg.ReadValue(root, key, "not exists").ToLong(), -1);
        }

        /// <summary>
        ///A test for ToLong
        ///</summary>
        [TestMethod()]
        public void ToLongTest_dw()
        {
            Assert.AreEqual(oReg.ReadValue(root, key, "dw").ToLong(), 51664468);
        }

        /// <summary>
        ///A test for ToLong
        ///</summary>
        [TestMethod()]
        public void ToLongTest_qw()
        {
            Assert.AreEqual(oReg.ReadValue(root, key, "qw").ToLong(), 36329457509);
        }

        /// <summary>
        ///A test for ToLong
        ///</summary>
        [TestMethod()]
        public void ToLongTest_exp()
        {
            Assert.AreEqual(oReg.ReadValue(root, key, "exp").ToLong(), -1);
        }
        #endregion Bin

        #region Test ToString
        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest_bin()
        {
            Assert.AreEqual(oReg.ReadValue(root, key, "bin").ToString(), "c-sharp ist scheisse");
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest_multi()
        {     
            Assert.AreEqual(oReg.ReadValue(root, key, "multi").ToString(), "aaa\0bbb\0ccc");
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest_regsz()
        {     
            Assert.AreEqual(oReg.ReadValue(root, key, "reg_sz").ToString(), "string123456");
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest_notexists()
        {     
            Assert.AreEqual(oReg.ReadValue(root, key, "not exists").ToString(), null);
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest_dw()
        {     
            Assert.AreEqual(oReg.ReadValue(root, key, "dw").ToString(), "51664468");
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest_qw()
        {     
            Assert.AreEqual(oReg.ReadValue(root, key, "qw").ToString(), "36329457509");
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest_exp()
        {     
            Assert.AreEqual(oReg.ReadValue(root, key, "exp").ToString(), "123456öaslkfjösaodjfäüsadojgfäsdk");
        }
        #endregion Test ToString

        #region Test ToStringArray
        /// <summary>
        ///A test for ToStringArray
        ///</summary>
        [TestMethod()]
        public void ToStringArrayTestMulti()
        {
            string[] mul = oReg.ReadValue(root, key, "multi").ToStringArray();
            string[] b = new string[] { "aaa", "bbb", "ccc" };
            Assert.AreEqual(mul.Length, b.Length);

            for (int i = 0; i < mul.Length; i++)
            {
                Assert.AreEqual(mul[i], b[i], i + " not equal");
            }
        }

        [TestMethod()]
        public void ToStringArrayTestNone()
        {
            string[] mul = oReg.ReadValue(root, key, "multisaefaw").ToStringArray();
            Assert.IsNull(mul);
        }

        [TestMethod()]
        public void ToStringArrayTestString()
        {
            string[] mul = oReg.ReadValue(root, key, "reg_sz").ToStringArray();
            string b = "string123456";
            Assert.AreEqual(mul[0], b);
            Assert.AreEqual(mul.Length, 1);
        }

        [TestMethod()]
        public void ToStringArrayTestExp()
        {
            string[] mul = oReg.ReadValue(root, key, "exp").ToStringArray();
            string b = "123456öaslkfjösaodjfäüsadojgfäsdk";
            Assert.AreEqual(mul[0], b);
            Assert.AreEqual(mul.Length, 1);
        }

        [TestMethod()]
        public void ToStringArrayTestBinary()
        {
            string[] mul = oReg.ReadValue(root, key, "bin").ToStringArray();
            Assert.IsNotNull(mul, "null");
            
            Assert.AreNotEqual(mul.Length, 0, "Length = 0");
            string b = "c-sharp ist scheisse";
            Assert.AreEqual(mul[0], b, "compare arrays");
            Assert.AreNotEqual(mul[0], "System.Byte[]");
            Assert.AreEqual(mul.Length, 1);
        }

        #endregion Test ToStringArray

        #region Test ToStringList
        /// <summary>
        ///A test for ToStringList
        ///</summary>
        [TestMethod()]
        public void ToStringListTest()
        {
            List<string> reg_szList = oReg.ReadValue(root, key, "reg_sz").ToStringList();
            string b = "string123456";
            Assert.AreEqual(reg_szList[0], b, "reg_sz test");

            List<string> reg_DWORD = oReg.ReadValue(root, key, "dw").ToStringList();
            string dw = "51664468";
            Assert.AreEqual(reg_DWORD[0], dw, "DWORD test");

            List<string> reg_QWORD = oReg.ReadValue(root, key, "qw").ToStringList();
            string qw = "36329457509";
            Assert.AreEqual(reg_QWORD[0], qw, "QWORD test");

            List<string> reg_EXP = oReg.ReadValue(root, key, "exp").ToStringList();
            string ex = "123456öaslkfjösaodjfäüsadojgfäsdk";
            Assert.AreEqual(reg_EXP[0], ex, "Expand test");

            List<string> reg_Multi = oReg.ReadValue(root, key, "multi").ToStringList();
            string mul = "aaa";
            Assert.AreEqual(reg_Multi[0], mul, "Multi test");

            List<string> reg_None = oReg.ReadValue(root, key, "multiadwadaw").ToStringList();
            string no = null;
            Assert.AreEqual(reg_None, no, "None test");

            List<string> reg_bin = oReg.ReadValue(root, key, "bin").ToStringList();
            string bin = "c-sharp ist scheisse";
            Assert.AreEqual(reg_bin[0], bin, "Binary test");
        }
        #endregion Test ToStringList

        #region Test keyName
        
        /// <summary>
        ///A test for keyName
        ///</summary>
        [TestMethod()]
        public void keyTest_Keyname()
        {            
            string sKeyname = @"asdfasdf\dummy";
            string sValuename = @"testvalue";
            RegObject target = oReg.ReadValue(RegistryHelper.ROOT_KEY.HKEY_CURRENT_USER, sKeyname, sValuename);
            Assert.AreEqual(target.KeyName, sValuename);
        }
        #endregion Test keyName

        #region test KeyPath
        /// <summary>
        ///A test for keyName
        ///</summary>
        [TestMethod()]
        public void keyTest_KeyPath()
        {
            string sKeyname = @"asdfasdf\dummy";
            string sValuename = @"testvalue";
            RegObject target = oReg.ReadValue(RegistryHelper.ROOT_KEY.HKEY_CURRENT_USER, sKeyname, sValuename);
            Assert.AreEqual(target.SubKey, sKeyname);
        }

        /// <summary>
        ///A test for keyName
        ///</summary>
        [TestMethod()]
        public void keyRootKeyTest_KeyReadTest()
        {
            string sKeyname = @"asdfasdf\dummy";
            string sValuename = @"testvalue";
            RegObject target = oReg.ReadValue(RegistryHelper.ROOT_KEY.HKEY_CURRENT_USER, sKeyname, sValuename);
            Assert.AreEqual(target.RootKey, RegistryHelper.ROOT_KEY.HKEY_CURRENT_USER);
        }

        #endregion test KeyPath

        #region Test PathTest
        /// <summary>
        ///A test for path
        ///</summary>
        [TestMethod()]
        public void pathTest()
        {
            RegObject target = new RegObject();
            string expected = string.Empty; 
            string actual;
            target.SubKey = expected;
            actual = target.SubKey;
            Assert.AreEqual(expected, actual);
        }
        #endregion Test PathTest

        #region Test ValueTest
        /// <summary>
        ///A test for value
        ///</summary>
        [TestMethod()]
        public void valueTest()
        {
            string sKeyname = @"asdfasdf\dummy";
            string sValuename = @"testvalue";
            RegObject target = oReg.ReadValue(RegistryHelper.ROOT_KEY.HKEY_CURRENT_USER, sKeyname, sValuename);
            Assert.AreEqual(target.KeyName, sValuename);
        }
        #endregion Test ValueTest

    }
}
