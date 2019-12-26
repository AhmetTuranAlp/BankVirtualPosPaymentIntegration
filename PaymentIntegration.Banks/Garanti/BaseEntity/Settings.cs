using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentIntegration.Banks.Garanti.BaseEntity
{
    /// <summary>
    /// Tüm çağrılarda kullanılacak ayarların tutulduğu sınıftır. 
    /// Bu sınıf üzerinde size özel parametreler fonksiyonlar arasında taşınabilir.
    /// </summary>
    public class Settings
    {
        public string Mode { get; set; }
        public string Version { get; set; }
        public string BaseUrl { get; set; }
        public string Password { get; set; }
    }
}
