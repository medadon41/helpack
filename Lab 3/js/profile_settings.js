let titleInput = document.getElementById("title")
let descInput = document.getElementById("description")
let goal = document.getElementById("goal")
let goalDesc = document.getElementById("goal-desc")
let fileUpload = document.getElementById("file-upload")
let currentImage = document.getElementById("current-image")
let categorySelect = document.getElementById("category-select")
let donTitle = document.getElementById("donation-title")
let donDesc = document.getElementById("donation-desc")

const currentUser = localStorage.getItem("currentUser")
const id = JSON.parse(currentUser).id
let profileId;

async function getProfileInfo() {
    const response = await fetch(`http://localhost:5043/api/Profile/${id}/Update`, {
        method: "GET",
        headers: { "Accept": "application/json" }
    })

    if (response.ok === true) {
        return await response.json()
    }
}

async function fillSettingsInfo() {
    let profileViewModel = await getProfileInfo()
    profileId = profileViewModel.id
    if (profileViewModel) {
        titleInput.value = profileViewModel.title
        descInput.value = profileViewModel.description
        goal.value = profileViewModel.goal
        goalDesc.value = profileViewModel.goalDescription

        if (profileViewModel.imageUrl !== null) {
            currentImage.src = profileViewModel.imageUrl
        }

        donTitle.value = profileViewModel.donationTitle
        donDesc.value = profileViewModel.donationTitle
    }
}

async function handleFormSubmit() {
    event.preventDefault()
    const file = fileUpload.files[0];
    const formData = new FormData();

    formData.append('image', file)
    formData.append('authorId', id)
    formData.append('id', profileId)
    formData.append('title', titleInput.value)
    formData.append('description', descInput.value)
    formData.append('goal', goal.value)
    formData.append('category', Number(categorySelect.value))
    formData.append('goalDescription', goalDesc.value)
    formData.append('donationTitle', donTitle.value)
    formData.append('donationDescription', donDesc.value)

    const profile = {
        authorId: id,
        id: profileId,
        title: titleInput.value,
        description: descInput.value,
        goal: goal.value,
        category: Number(categorySelect.value),
        goalDescription: goalDesc.value,
        donationTitle: donTitle.value,
        donationDescription: donDesc.value
    }


    const response = await fetch(`http://localhost:5043/api/Profile/${profileId}`, {
        method: "PUT",
        body: formData
    })

    let a = await response.json()
    console.log(a)
    if (response.ok === true) {
        let pr = await response.json()
        console.log(response)
    }
}

fillSettingsInfo()