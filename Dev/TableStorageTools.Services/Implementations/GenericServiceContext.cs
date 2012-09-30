using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using TableStorageTools.Model;
using TableStorageTools.Services.Interfaces;

namespace TableStorageTools.Services.Implementations
{
    public class GenericServiceContext : TableServiceContext, IGenericServiceContext
    {
        private static XNamespace _atomNs = "http://www.w3.org/2005/Atom";
        private static XNamespace _dataNs = "http://schemas.microsoft.com/ado/2007/08/dataservices";
        private static XNamespace _metadataNs = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

        public GenericServiceContext(CloudStorageAccount storageAccount)
            : base(storageAccount.TableEndpoint.ToString(), storageAccount.Credentials)
        {
            this.IgnoreMissingProperties = true;
            this.ReadingEntity += GenericServiceContext_ReadingEntity;
            this.WritingEntity += GenericServiceContext_WritingEntity;
        }

        public List<AzureGenericEntity> GetEntities(string tableName)
        {
            var query = (from r in this.CreateQuery<AzureGenericEntity>(tableName)
                         select r).AsTableServiceQuery();

            List<AzureGenericEntity> result = new List<AzureGenericEntity>();
            ResultContinuation token = null;

            result.AddRange(query.Execute(token));

            while (token != null)
            {
                result.AddRange(query.Execute(token));
            }

            return result;
        }

        public void SaveEntities(string tableName, List<AzureGenericEntity> entities, int count)
        {
            if (entities != null)
            {
                int i = 0;
                List<AzureGenericEntity> split = entities.Skip(count * i).Take(count).ToList();

                while (split.Any())
                {
                    i++;
                    split.ForEach(e => this.AddObject(tableName, e));
                    this.SaveChanges();

                    split = entities.Skip(count * i).Take(count).ToList();
                }
            }
        }

        private void GenericServiceContext_ReadingEntity(object sender, System.Data.Services.Client.ReadingWritingEntityEventArgs e)
        {
            AzureGenericEntity entity = e.Entity as AzureGenericEntity;
            if (entity == null)
            {
                return;
            }
            var q = from p in e.Data.Element(_atomNs + "content")
                                    .Element(_metadataNs + "properties")
                                    .Elements()
                    select new
                    {
                        Name = p.Name.LocalName,
                        IsNull = string.Equals("true", p.Attribute(_dataNs + "null") == null ? null : p.Attribute(_metadataNs + "null").Value, StringComparison.OrdinalIgnoreCase),
                        TypeName = p.Attribute(_dataNs + "type") == null ? null : p.Attribute(_metadataNs + "type").Value,
                        p.Value
                    };

            foreach (var dp in q)
            {
                entity.Properties.Add(new Tuple<string, object, object>(dp.Name, dp.TypeName ?? "Edm.String", dp.Value));
            }
        }


        private void GenericServiceContext_WritingEntity(object sender, System.Data.Services.Client.ReadingWritingEntityEventArgs e)
        {
            AzureGenericEntity entity = e.Entity as AzureGenericEntity;

            if (entity == null)
            {
                return;
            }

            XElement properties = e.Data.Descendants(_metadataNs + "properties").First();

            foreach (var property in entity.Properties.Skip(3))
            {
                var xmlProperty = new XElement(_dataNs + property.Item1, property.Item3);
                xmlProperty.Add(new XAttribute(_metadataNs + "type", property.Item2));
                properties.Add(xmlProperty);
            }


            bool finish = true;
        }


        private static Type GetType(string type)
        {
            if (type == null)
                return typeof(string);

            switch (type)
            {
                case "Edm.String": return typeof(string);
                case "Edm.Byte": return typeof(byte);
                case "Edm.SByte": return typeof(sbyte);
                case "Edm.Int16": return typeof(short);
                case "Edm.Int32": return typeof(int);
                case "Edm.Int64": return typeof(long);
                case "Edm.Double": return typeof(double);
                case "Edm.Single": return typeof(float);
                case "Edm.Boolean": return typeof(bool);
                case "Edm.Decimal": return typeof(decimal);
                case "Edm.DateTime": return typeof(DateTime);
                case "Edm.Binary": return typeof(byte[]);
                case "Edm.Guid": return typeof(Guid);

                default: throw new NotSupportedException("Not supported type " + type);
            }
        }

    }
}
