using System;
using System.Threading;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TableStorageTools.Services.Implementations;
using TableStorageTools.Services.Interfaces;

namespace TableStorageTools.Tests
{
    [TestClass]
    public class TableStorageServiceTest
    {
        private IUnityContainer _container = new UnityContainer();
        private TimeSpan _timeOut = TimeSpan.FromSeconds(10);

        [TestInitialize]
        public void StartUp()
        {
            Services.UnityRoot.Configure(_container);
        }

        [TestMethod]
        public void CloudStorageAccountIsNotDefined()
        {
            var service = _container.Resolve<ITableStorageService>();
            
            Assert.IsNotNull(service);

            var result = service.IsValidStorageAccount();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CloudStorageIsIncorrect()
        {
            var service = _container.Resolve<ITableStorageService>(new ParameterOverride("storageAccount", "incorrectValue"));

            Assert.IsNotNull(service);

            var result = service.IsValidStorageAccount();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CloudStorageIsDevelopmentStorage()
        {
            var service = _container.Resolve<ITableStorageService>(new ParameterOverride("storageAccount", "UseDevelopmentStorage=true"));

            Assert.IsNotNull(service);

            var result = service.IsValidStorageAccount();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CloudStorageIsInWindowsAzure()
        {
            var service = _container.Resolve<ITableStorageService>(new ParameterOverride("storageAccount", "DefaultEndpointsProtocol=https;AccountName=blogwilfriedwoivre;AccountKey=JyRZfh+8Llph4Bd6hv+nw1wX4GBMYiuZCgI/3PCCy7ZwFZ8wApHimP8lYT4vy/BZgrAUX9ROgOTm73sKLxDwMQ=="));

            Assert.IsNotNull(service);

            var result = service.IsValidStorageAccount();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ListTableWhenStorageAccountIsNotDefined()
        {
            var service = _container.Resolve<ITableStorageService>();

            Assert.IsNotNull(service);

            var awaiter = service.GetTablesNames(null, 3).GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                if (awaiter.IsCompleted)
                {
                    var result = awaiter.GetResult();

                    Assert.IsNull(result);
                }
            });

            Thread.Sleep(_timeOut);
            if (!awaiter.IsCompleted)
            {
                Assert.Inconclusive();
            }
        }

        [TestMethod]
        public void ListTableWhenStorageAccountIsIncorrect()
        {
            var service = _container.Resolve<ITableStorageService>(new ParameterOverride("storageAccount", "incorrectValue"));

            Assert.IsNotNull(service);

            var awaiter = service.GetTablesNames(null, 3).GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                if (awaiter.IsCompleted)
                {
                    var result = awaiter.GetResult();
                 
                    Assert.IsNull(result);
                }
            });

            Thread.Sleep(_timeOut);
            if (!awaiter.IsCompleted)
            {
                Assert.Inconclusive();
            }
        }

        [TestMethod]
        public void ListTableWhenStorageAccountIsDevelopmentStorage()
        {
            var service = _container.Resolve<ITableStorageService>(new ParameterOverride("storageAccount", "UseDevelopmentStorage=true"));

            Assert.IsNotNull(service);

            var awaiter = service.GetTablesNames(null, 3).GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                if (awaiter.IsCompleted)
                {
                    var result = awaiter.GetResult();
                    Assert.IsNotNull(result);

                    Assert.IsTrue(result.Count == 0);
                }
            });

            Thread.Sleep(_timeOut);
            if (!awaiter.IsCompleted)
            {
                Assert.Inconclusive();
            }
        }

        [TestMethod]
        public void ListTableWhenStorageAccountIsInWindowsAzure()
        {
            var service = _container.Resolve<ITableStorageService>(new ParameterOverride("storageAccount", "DefaultEndpointsProtocol=https;AccountName=blogwilfriedwoivre;AccountKey=JyRZfh+8Llph4Bd6hv+nw1wX4GBMYiuZCgI/3PCCy7ZwFZ8wApHimP8lYT4vy/BZgrAUX9ROgOTm73sKLxDwMQ=="));

            Assert.IsNotNull(service);

            var awaiter = service.GetTablesNames(null, 3).GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                if (awaiter.IsCompleted)
                {
                    var result = awaiter.GetResult();
                    Assert.IsNotNull(result);

                    Assert.IsTrue(result.Count == 8);
                }
            });

            Thread.Sleep(_timeOut);
            if (!awaiter.IsCompleted)
            {
                Assert.Inconclusive();
            }
        }
    }
}
