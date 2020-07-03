using PE.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PE.API.Helpers
{
    public class IdHelper
    {
        public static string FindAssignableId(List<string> input)
        {
            int idCandidate = 1;
            var ids = input.Select(x => Convert.ToInt32(x));


            foreach (var id in ids)
            {
                if (idCandidate == id)
                {
                    idCandidate++;
                }
            }

            return idCandidate.ToString();
        }
    }
}
