let errorMessage = document.querySelector(".error-message")
let emailInput = document.getElementById("email")
let passwordInput = document.getElementById("password")

async function handleLoginForm() {
    event.preventDefault()


    let loginViewModel = {
        email: emailInput.value,
        password: passwordInput.value.replace(" ", "")
    }

    const response = await fetch("http://localhost:5043/api/Users/Login", {
        method: "POST",
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(loginViewModel)
    })

    console.log(loginViewModel)

    if (response.ok === true) {
        let user = await response.json()
        localStorage.setItem('currentUser', JSON.stringify({id: user.id, userName: user.userName}))
        window.location = `main.html`
    }
    else {
        console.log(await response.json())
        if (response.status === 404)
            errorMessage.innerHTML = "User not found"
        else if (response.status === 400)
            errorMessage.innerHTML = "Incorrect password"
    }

    return false
}

document.getElementById("recovery-link").onclick = async ()=> {
    let email = document.getElementById("email").value
    const response = await fetch(`http://localhost:5043/api/Users/Recover/${email}`, {
        method: "GET",
        headers: { "Accept": "application/json" }
    })

    if (response.ok === true) {
        errorMessage.innerHTML = "Temporary password has been sent to your email."
    } else {
        errorMessage.innerHTML = "User not found"
    }
}