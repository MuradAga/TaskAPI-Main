
async function login() {
    let username = document.getElementById("username").value;
    let password = document.getElementById("pass").value;

    let response = await fetch(`https://localhost:7293/api/Users/login?username=${username}&password=${password}`, {
        method: "GET", // *GET, POST, PUT, DELETE, etc.
        mode: "cors", // no-cors, *cors, same-origin
        cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
        headers: {
          "Content-Type": "application/json",
        },
      });
      if (response.status == 200) {
    const data = response.json();
    data.then((user) =>

    document.getElementById("result").style.display = "block",
    document.getElementById("error").style.display = "none",

    );
}
else if(response.status == 404){
    document.getElementById("error").style.display = "block";
    document.getElementById("result").style.display = "none";
}
}

async function register() {
  let phone = document.getElementById("phone").value;
  let fullname = document.getElementById("fullname").value;
  let username = document.getElementById("username").value;
  let password = document.getElementById("pass").value;
  let code = document.getElementById("code").value;

  let model = {PhoneOrEmail: phone, FullName: fullname,Username: username, Password: password, Code: code};
  let response = await fetch("https://localhost:7293/api/Users/register", {
        method: "POST", // *GET, POST, PUT, DELETE, etc.
        mode: "cors", // no-cors, *cors, same-origin
        cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
        body: JSON.stringify(model),
        headers: {
          "Content-Type": "application/json",
        },
      });

    if (response.status == 200) {
      window.location = "login.html"
    }
    else if(response.status == 400){
      document.getElementById("response").style.display = "block";
      response.text().then(data => document.getElementById("response").innerHTML = data);
    }
}


async function resetPassword() {
  let email = document.getElementById("phone").value;
  let newPassword = document.getElementById("pass1").value;
  let repeatNewPassword = document.getElementById("pass2").value;
  let verificationCode = document.getElementById("code").value;

    let model = {Email: email, NewPassword: newPassword,RepeatNewPassword: repeatNewPassword, VerificationCode: verificationCode};

    let response = await fetch("https://localhost:7293/api/Users/forgotPassword", {
        method: "PATCH", // *GET, POST, PUT, DELETE, etc.
        mode: "cors", // no-cors, *cors, same-origin
        cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
        body: JSON.stringify(model),
        headers: {
          "Content-Type": "application/json",
        },
      });

      if (response.status == 200) {
        window.location = "login.html"
    }
    else if(response.status == 400){
      document.getElementById("response").style.display = "block";
    response.text().then(data => document.getElementById("response").innerHTML = data);
      }
}


async function sendCodeToEmail() {
  let emailOrPhone = document.getElementById("phone").value;

  await fetch(`https://localhost:7293/api/Users/sendCodeToEmail?email=${emailOrPhone.replace("@", "%40")}`, {
        method: "POST", // *GET, POST, PUT, DELETE, etc.
        mode: "cors", // no-cors, *cors, same-origin
        cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
        headers: {
          "Content-Type": "application/json",
        },
      });
}