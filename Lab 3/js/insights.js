"use strict"

class DonationViewModel {
    constructor(name, description, amount, date) {
        this.name = name;
        this.message = description;
        this.amount = amount;
        this.date = date;
    }
}

let mockDonations = [new DonationViewModel("Donator #1", "просто забери мои деньги :)", 200, "12.03.2023"),
    new DonationViewModel("Donator #1", "просто забери мои деньги :)", 200, "12.03.2023"),
    new DonationViewModel("Donator #1", "просто забери мои деньги :)", 200, "12.03.2023"),
    new DonationViewModel("Donator #1", "просто забери мои деньги :)", 200, "12.03.2023"),
    new DonationViewModel("Donator #1", "просто забери мои деньги :)", 200, "12.03.2023"),
    new DonationViewModel("Donator #1", "просто забери мои деньги :)", 200, "12.03.2023"),
    new DonationViewModel("Donator #1", "просто забери мои деньги :)", 200, "12.03.2023"),
    new DonationViewModel("Donator #2", "просто забери мои деньги :)", 100, "11.03.2023")];

function pushDonationCards() {
    let cards_container = document.querySelector(".cards")
    mockDonations.forEach(item => {
        let card = document.createElement("div")
        card.className = "card"

        let donator_name = document.createElement("h3")
        donator_name.innerHTML = `${item.name}`
        card.appendChild(donator_name)

        let b_amount = document.createElement("b")
        let amount = document.createElement("b")
        b_amount.innerHTML = "Amount: "
        card.appendChild(b_amount)
        amount.innerHTML = `\$${item.amount}`
        amount.style.fontWeight = "Normal"
        card.appendChild(amount)

        card.appendChild(document.createElement("br"))

        let b_date = document.createElement("b")
        let date = document.createElement("b")
        b_date.innerHTML = "Date: "
        card.appendChild(b_date)
        date.innerHTML = `${item.date}`
        date.style.fontWeight = "Normal"
        card.appendChild(date)

        let desc = document.createElement("p")
        desc.innerHTML = item.message
        card.appendChild(desc)

        cards_container.appendChild(card)
    })
}

function distinctElementsByName(arr) {
    const distincted = arr.reduce((set, obj) => {
        set.add(obj.name);
        return set;
    }, new Set());

    return distincted.size;
}

function getTotalPerDay() {
    const resultArray = Object.values(mockDonations.reduce((acc, {date, amount}) => {
        acc[date] = acc[date] || { date, amount: 0 };
        acc[date].amount += amount;
        return acc;
    }, {}))
        .sort((a, b) => b.amount - a.amount);

    return resultArray
}


function fillUserStats() {
    let totalAmount = mockDonations.reduce((acc, curr) => acc + curr.amount, 0)
    let raised = document.getElementById("user-raised")
    raised.innerHTML = `<b>Raised so far: </b> \$${totalAmount}`

    let totalDonators = distinctElementsByName(mockDonations)
    let donators = document.getElementById("donators-sum")
    donators.innerHTML = `<b>Total donators: </b> ${totalDonators}`

}

function fillEstimatedStats() {
    let totalSum = mockDonations.reduce((acc, curr) => acc + curr.amount, 0);
    let totalPerDay = getTotalPerDay()

    let avg = document.getElementById("avg-day")
    avg.innerHTML = `<b>Average donation: </b> \$${totalSum/mockDonations.length}`

    let highest = document.getElementById("highest-day")
    highest.innerHTML = `<b>Highest day: </b> ${totalPerDay[0].date}`

    let lowest = document.getElementById("lowest-day")
    lowest.innerHTML = `<b>Lowest day: </b> ${totalPerDay[totalPerDay.length-1].date}`
}

fillUserStats()
fillEstimatedStats()
pushDonationCards()