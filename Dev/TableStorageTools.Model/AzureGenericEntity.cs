using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.StorageClient;

namespace TableStorageTools.Model
{
    [DataServiceKey("PartitionKey", "RowKey")]
    public class AzureGenericEntity : TableServiceEntity
    {
        private List<Tuple<string, object, object>> _properties = new List<Tuple<string, object, object>>();
        public List<Tuple<string, object, object>> Properties
        {
            get
            {
                return _properties;
            }
            set
            {
                _properties = value;
            }
        }
    }
}
