#pragma checksum "/Users/zentoo/Code/dataentry/DataEntry/Views/Account/Login.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6f8993cedf9f37022a236e8d63ec4a81455e2946"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Account_Login), @"mvc.1.0.view", @"/Views/Account/Login.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6f8993cedf9f37022a236e8d63ec4a81455e2946", @"/Views/Account/Login.cshtml")]
    public class Views_Account_Login : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\n");
#nullable restore
#line 5 "/Users/zentoo/Code/dataentry/DataEntry/Views/Account/Login.cshtml"
  
    string baseUrl = $"https://{Context.Request.Host.Value}";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<h1>Login</h1>

<form onsubmit=""return login(event)"">
    <label for=""username"">UserName</label>
    <input type=""text"" id=""username"" title=""UserName"" />
    <label for=""password"">Password</label>
    <input type=""text"" id=""password"" title=""Password"" />
    <input type=""submit"" value=""Submit"" />
</form>


<script src=""https://code.jquery.com/jquery-3.4.1.min.js"" integrity=""sha256-CSXorXvZcTkaix6Yvo6HppcZGetbYMGWSFlBw8HfCJo="" crossorigin=""anonymous""></script>

<script type=""text/javascript"">
    function login(event) {
        var nonce = 'e3fd62f2-78f3-46fb-874f-ebe5985194ed';

        $.ajax({
            type: ""POST"",
            contentType: ""application/json"",
            url: '");
#nullable restore
#line 29 "/Users/zentoo/Code/dataentry/DataEntry/Views/Account/Login.cshtml"
             Write(Url.Action("Login", "Account"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"',
            dataType: ""json"",
            data: JSON.stringify({
                username: $('#username').val(),
                password: $('#password').val(),
                nonce: nonce
            }),
            error: function (result) {
                // TODO: show error
                console.log(""error"");
            },
            success: function (result) {
                // set local storage with token
                localStorage.setItem('adal.login.request', '");
#nullable restore
#line 42 "/Users/zentoo/Code/dataentry/DataEntry/Views/Account/Login.cshtml"
                                                       Write(baseUrl);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"');
                localStorage.setItem('adal.token.keys', result.clientId);
                localStorage.setItem('adal.state.login', '95933b0f-7ec2-45d3-9892-ce56ec31bef2||');
                localStorage.setItem('adal.nonce.idtoken', `${nonce}||`);

                // navigate to base domain with new token
                window.location.href = `/#id_token=${result.token}&state=95933b0f-7ec2-45d3-9892-ce56ec31bef2&session_state=${result.sessionState}`;
            }
        });

        // return false to prevent default behavior
        return false;
    }
</script>
");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
