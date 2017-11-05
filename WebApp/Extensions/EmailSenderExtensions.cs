using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using WebApp.Services;

namespace WebApp.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "邮箱验证",
                $"欢迎注册成为微软中国开发者用户，<br/>请点击以下链接验证您的邮箱: <a href='{HtmlEncoder.Default.Encode(link)}'>点此验证</a>");
        }
    }
}
