const urlParams = new URLSearchParams(window.location.search);
const id = urlParams.get('id');

async function getDonationPage() {
    const response = await fetch(`http://localhost:5043/api/Profile/${id}/Donate`, {
        method: "GET",
        headers: { "Accept": "application/json" }
    })

    let profile;

    if (response.ok === true) {
        profile = await response.json()
    }

    console.log(profile)

    let donationTitle = document.getElementById("donation-title")
    donationTitle.innerHTML = profile.donationTitle

    let donationDesc = document.getElementById("donation-desc")
    donationDesc.innerHTML = profile.donationDescription

    let donationButton = document.querySelector(".donation-button")
    donationButton.href = `profile.html?id=${profile.id}`
}

async function handleFormSubmit() {
    event.preventDefault()
    let donationForm = document.forms["donation"]
    let author = donationForm["author"].value
    let message = donationForm["message"].value
    let amount = donationForm["amount"].value

    const newDonation = {
        receiverId: id,
        amount: amount,
        name: author,
        message: message
    }


    let response =  await fetch(`http://localhost:5043/api/Donations`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(newDonation)
    })

    window.location = `profile.html?id=${id}`

}

getDonationPage()