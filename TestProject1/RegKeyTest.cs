using RegistryClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for RegKeyTest and is intended
    ///to contain all RegKeyTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RegKeyTest
    {

        const string sKeyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer";
        const string sTestKey = @"SOFTWARE\SIMAC\TestMa";
        public RegistryClass.RegistryHelper.ROOT_KEY root = RegistryHelper.ROOT_KEY.HKEY_CURRENT_USER;
        public RegistryHelper oReg = new RegistryHelper();


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


        /// <summary>
        ///A test for RegKey Constructor
        ///</summary>
        [TestMethod()]
        public void RegKeyConstructorTest()
        {
         
            //Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for readValues
        ///</summary>
        [TestMethod()]
        public void readValuesTest()
        {
            string readTestKey = "ValueReadTestKey";
            string readTestValue = "readTest";
            bool result = oReg.WriteValue(root, sTestKey, RegistryHelper.VALUE_TYPE.REG_SZ, readTestKey, readTestValue);
            Assert.AreEqual(result, true, "Write value");
            Assert.AreEqual((string)oReg.ReadValue(root, sTestKey, readTestKey).Value, readTestValue, "Read value");
        }

        /// <summary>
        ///A test for Values
        ///</summary>
        [TestMethod()]
        public void ValuesTest()
        {
            string SubTestKey = sTestKey + "\\subkey";
            Assert.IsTrue(oReg.DeleteKey(root, SubTestKey));
            Assert.IsTrue(oReg.DeleteKey(root,sTestKey));
            
            Assert.IsTrue(oReg.CreateKey(root, sTestKey));
            Assert.IsTrue(oReg.CreateKey(root, SubTestKey));

            for (int i = 0; i < 10; i++)
            {
                Assert.IsTrue(oReg.WriteValue(root, SubTestKey, RegistryHelper.VALUE_TYPE.REG_SZ, "a" + i.ToString(), i.ToString()));
            }

            RegKey ok = oReg.GetKey(root, SubTestKey, true, true);
            Assert.IsTrue(ok.Values.Count > 0);
            Assert.AreEqual(ok.Values.Count, 10);
            
        }

        /// <summary>
        ///A test for oParentObject
        ///</summary>
        [TestMethod()]
        public void oParentObjectTest()
        {
            RegKey ok = oReg.GetKey(RegistryHelper.ROOT_KEY.HKEY_LOCAL_MACHINE, sKeyName, false, false);
            Assert.AreSame(ok.oParentObject, oReg);            
        }


        
        /// <summary>
        ///A test for oParentObject
        ///</summary>
        [TestMethod()]
        public void KeyFULLReadTest_noValues_noSubKeys()
        {
            RegKey ok = oReg.GetKey(RegistryHelper.ROOT_KEY.HKEY_LOCAL_MACHINE, sKeyName, false, false);
            Assert.AreEqual(ok.Values, null);
            Assert.AreEqual(ok.SubKeys, null);
        }

        [TestMethod()]
        public void KeyFULLReadTest_noValues_SubKeys()
        {
            RegKey ok = oReg.GetKey(RegistryHelper.ROOT_KEY.HKEY_LOCAL_MACHINE, sKeyName, false, true);
            Assert.AreEqual(ok.Values, null);            
            Assert.IsTrue(ok.SubKeys.Count > 1);
        }

        [TestMethod()]
        public void KeyFULLReadTest_Values_noSubKeys()
        {
            RegKey ok = oReg.GetKey(RegistryHelper.ROOT_KEY.HKEY_LOCAL_MACHINE, sKeyName, true, false);
            Assert.IsTrue(ok.Values.Count > 1);
            Assert.AreEqual(ok.SubKeys, null);
        }

        [TestMethod()]
        public void KeyFULLReadTest_Values_SubKeys()
        {
            RegKey ok = oReg.GetKey(RegistryHelper.ROOT_KEY.HKEY_LOCAL_MACHINE, sKeyName, true, true);
            Assert.IsTrue(ok.Values.Count > 0, " mainkey has values");
            Assert.IsTrue(ok.SubKeys.Count > 0, "mainkey has subkeys");
            Assert.IsTrue(ok.SubKeys[0].Values.Count > 0,"subkey 0 has values");
            Assert.IsTrue(ok.SubKeys[0].SubKeys.Count > 0,"subkey 0 has subkeys");
        }
    }
}
