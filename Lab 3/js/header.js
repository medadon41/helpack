const currentUser = localStorage.getItem("currentUser")

let headerHello = document.getElementById("header-hello")
let headerSettings = document.getElementById("header-settings")
let headerAuth = document.getElementById("header-auth")

async function fillHeaderLinks() {
    if (currentUser !== null) {
        const user = JSON.parse(currentUser);
        headerHello.innerHTML = `Hello, ${user.userName}!`
        headerAuth.onclick = () => {
            localStorage.clear()
        }
        headerAuth.href = "main.html"
        headerSettings.href = "account_settings.html"
        let profileId = await getProfileInfo(user.id)
        headerHello.href = `profile.html?id=${profileId}`
    } else {
        headerHello.innerHTML = `Hello, Stranger`
        headerHello.href = ""
        headerSettings.style.display = "none";
        headerAuth.innerHTML = 'Log In'
        headerAuth.href = "login.html"
    }
}

async function getProfileInfo(id) {
    const response = await fetch(`http://localhost:5043/api/Profile/${id}/Update`, {
        method: "GET",
        headers: { "Accept": "application/json" }
    })

    if (response.ok === true) {
        const profile = await response.json()
        return profile.id
    }
}

fillHeaderLinks()