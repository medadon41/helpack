let emailInput = document.getElementById("email")
let oldPass = document.getElementById("old-password")
let newPass = document.getElementById("new-password")
let newPassConfirm = document.getElementById("new-password-2")

const currentUser = localStorage.getItem("currentUser")
const id = JSON.parse(currentUser).id

async function getUserInfo() {
    const response = await fetch(`http://localhost:5043/api/Users/${id}`, {
        method: "GET",
        headers: { "Accept": "application/json" }
    })

    if (response.ok === true) {
        return await response.json()
    }
}

async function fillSettingsInfo() {
    let account = await getUserInfo()

    if (account) {
        emailInput.value = account.email
    }
}

async function handleFormSubmit() {
    event.preventDefault()
    let email = emailInput.value
    let password = oldPass.value
    let newPassword = newPass.value
    let newPassword2 = newPassConfirm.value
    let userName = JSON.parse(currentUser).userName

    if (!(await validateForm(userName, email, password, newPassword, newPassword2))) {
        return false
    }

    const updateUserModel = {
        id: id,
        email: email,
        userName: userName,
        password: newPassword
    }

    const response = await fetch(`http://localhost:5043/api/Users/${id}`, {
        method: "PUT",
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(updateUserModel)
    })

    window.location = `profile.html`
}

async function validateForm(userName, email, password, newPassword, newPassword2) {
    const passwordRegex = /^[a-zA-Z0-9_]{1,20}$/;
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    const loginViewModel = {
        userName: userName,
        password: password
    }

    const response = await fetch("http://localhost:5043/api/Users/Login", {
        method: "POST",
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(loginViewModel)
    })

    if (response.ok !== true)
        return false

    if(!passwordRegex.test(newPassword))
        return false

    if (newPassword !== newPassword2)
        return false

    return emailRegex.test(email);
}

fillSettingsInfo()