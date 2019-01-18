<<<<<<< HEAD
﻿using System.Threading.Tasks;

namespace Volo.Abp.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string phoneNumber, string text);
    }
}
=======
﻿using System.Threading.Tasks;

namespace Volo.Abp.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(SmsMessage smsMessage);
    }
}
>>>>>>> upstream/master
