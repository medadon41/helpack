let errorMessage = document.querySelector(".error-message")

async function checkUserExists(email, userName) {
    const user = {
        userName: userName,
        email: email
    }
    const response= await fetch(`http://localhost:5043/api/Users/Check`, {
        method: "POST",
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(user)
    })

    return response.status !== 404;
}

async function validateForm(email, userName, password, confirmPass) {
    const passwordRegex = /^[a-zA-Z0-9_]{1,20}$/;
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if(!passwordRegex.test(password)) {
        errorMessage.innerHTML = "Password can contain letters, numbers, underscore symbol and must not be longer that 20 symbols."
        return false
    }

    if (await checkUserExists(email, userName) === true) {
        errorMessage.innerHTML = "User with such or/and email already exists"
        return false
    }

    if (password !== confirmPass) {
        errorMessage.innerHTML = "Passwords don't match"
        return false
    }

    if(!emailRegex.test(email)) {
        errorMessage.innerHTML = "Invalid e-mail input"
        return false
    }

    return true
}

async function handleRegisterForm() {
    event.preventDefault()
    let registerForm = document.forms["register"]
    let email = registerForm["email"].value
    let userName = registerForm["user-name"].value
    let password = registerForm["password"].value
    let confirmPass = registerForm["password2"].value

    const newUser = {
        email: email,
        userName: userName,
        password: password
    }

    if (!(await validateForm(email, userName, password, confirmPass))) {
        return false
    }

    const response = await fetch(`http://localhost:5043/api/Users`, {
        method: "POST",
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(newUser)
    })

    let user;
    if (response.ok === true) {
        user = await response.json()
    }

    localStorage.setItem('currentUser', JSON.stringify({id: user.id, userName: user.userName}))
    window.location = `profile_settings.html`
}