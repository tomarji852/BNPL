using BNPL.EntityModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BNPL.Services
{
    public class CreditLimitService
    {
        private List<CreditLimitEntity> _creditLimitEntities;

        public List<CreditLimitEntity> LoadCreditLimitData()
        {
            List<CreditLimitEntity> CustomerCreditLimits = new List<CreditLimitEntity>();
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\creditLimitData.json");
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                CustomerCreditLimits = JsonConvert.DeserializeObject<List<CreditLimitEntity>>(json);
            }

            return CustomerCreditLimits;
        }
    }
}
