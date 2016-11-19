using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hintme.Models
{
    public interface IHintRepository
    {
        IEnumerable<Hint> GetHints();
        void SaveHint(Hint hint);
    }
}
