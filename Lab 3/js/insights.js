"use strict"

var myHeaders = new Headers();
myHeaders.append("apikey", "jqBnv15QQtOKrOjhqUslGJc6zCeI1UiZ");

var requestOptions = {
    method: 'GET',
    redirect: 'follow',
    headers: myHeaders
};

let donationViewModels = [];

function pushDonationCards() {
    let cards_container = document.querySelector(".cards")
    donationViewModels.forEach(item => {
        let card = document.createElement("div")
        card.className = "card"

        let donator_name = document.createElement("h3")
        donator_name.innerHTML = `${item.author}`
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

async function getInsightsInfo() {
    const urlParams = new URLSearchParams(window.location.search);
    const id = urlParams.get('id');
    const response = await fetch(`http://localhost:5043/api/Profile/${id}/Insights`, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });


    let profile;

    if (response.ok === true) {
        profile = await response.json()
        donationViewModels = profile.donations
    }

    fillUserStats(profile.registrationDate, profile.author)
    fillEstimatedStats()
    pushDonationCards()

}

function distinctElementsByName(arr) {
    const distincted = arr.reduce((set, obj) => {
        set.add(obj.name);
        return set;
    }, new Set());

    return distincted.size;
}

function getTotalPerDay() {
    const resultArray = Object.values(donationViewModels.reduce((acc, {date, amount}) => {
        acc[date] = acc[date] || { date, amount: 0 };
        acc[date].amount += amount;
        return acc;
    }, {}))
        .sort((a, b) => b.amount - a.amount);

    return resultArray
}

function distinctAndSort() {
    return Object.values(donationViewModels.reduce((acc, {date, amount}) => {
        acc[date] = acc[date] || {date, amount: 0};
        acc[date].amount += amount;
        return acc;
    }, {}))
}
function fillUserStats(userDate, userName) {
    let userNameTag = document.getElementById("user-name")
    userNameTag.innerHTML = userName

    let joined = document.getElementById("user-joined")
    joined.innerHTML = `<b>Date joined: </b> ${userDate}`

    let totalAmount = donationViewModels.reduce((acc, curr) => acc + curr.amount, 0)
    let raised = document.getElementById("user-raised")
    raised.innerHTML = `<b>Raised so far: </b> \$${totalAmount}`

    let totalDonators = distinctElementsByName(donationViewModels)
    let donators = document.getElementById("donators-sum")
    donators.innerHTML = `<b>Total donators: </b> ${totalDonators}`

}

function fillEstimatedStats() {
    let totalSum = donationViewModels.reduce((acc, curr) => acc + curr.amount, 0);
    let totalPerDay = getTotalPerDay()

    // charts coming in

    let statsData = distinctAndSort()

    const amounts = statsData.map(obj => obj.amount);
    const dates = statsData.map(obj => obj.date);

    console.log(amounts, dates)

    let statsGraph = document.querySelector(".stats__graph")

    const myChart = new Chart(statsGraph, {
        type: 'line',
        data: {
            labels: dates,
            datasets: [{
                label: null,
                data: amounts,
                borderWidth: 1,
                tension: 0.4,
                borderColor: '#15786b',
                backgroundColor: '#92C3BD',
                fill: "start",

            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });

    let avg = document.getElementById("avg-day")
    avg.innerHTML = `<b>Average donation: </b> \$${totalSum/donationViewModels.length}`

    let highest = document.getElementById("highest-day")
    highest.innerHTML = `<b>Highest day: </b> ${totalPerDay[0].date}`

    let lowest = document.getElementById("lowest-day")
    lowest.innerHTML = `<b>Lowest day: </b> ${totalPerDay[totalPerDay.length-1].date}`
}

let convertButton = document.getElementById("convert-button")
convertButton.addEventListener("click", function () {
    const convertFrom = document.getElementById("data-input").value
    const currencyFrom = document.getElementById("select-data").value
    const currencyTo = document.getElementById("select-result").value

    let convertTo = document.getElementById("data-result")

    fetch(`https://api.apilayer.com/fixer/convert?to=${currencyTo}&from=${currencyFrom}&amount=${convertFrom}`, requestOptions)
        .then(response => response.json())
        .then(result => convertTo.value = result.result)
        .catch(error => console.log('error', error));
})

async function getCurrencyInfo() {
    const response = await fetch("https://api.apilayer.com/fixer/latest?symbols=EUR%2CBYN%2CRUB%2CUAH%2CBTC&base=USD", requestOptions)

    let result
    if (response.ok === true) {
        result = await response.json()
        console.log(result)
    }

    let dateUpd = document.getElementById("date-update")
    const date = new Date();
    const formattedDate = `${date.getDate().toString().padStart(2, '0')}-${(date.getMonth() + 1).toString().padStart(2, '0')}-${date.getFullYear()} ${date.getHours().toString().padStart(2, '0')}:${date.getMinutes().toString().padStart(2, '0')}`;

    dateUpd.innerHTML = `as on ${formattedDate}`
    let eurRate = document.getElementById("eur-rate")
    let bynRate = document.getElementById("byn-rate")
    let rubRate = document.getElementById("rub-rate")
    let btcRate = document.getElementById("btc-rate")
    let uahRate = document.getElementById("uah-rate")

    eurRate.innerHTML = `${result.rates.EUR.toFixed(2)} EUR`
    bynRate.innerHTML = `${result.rates.BYN.toFixed(2)} BYN`
    rubRate.innerHTML = `${result.rates.RUB.toFixed(2)} RUB`
    btcRate.innerHTML = `${result.rates.BTC.toFixed(6)} BTC`
    uahRate.innerHTML = `${result.rates.UAH.toFixed(2)} UAH`
}

getInsightsInfo()
getCurrencyInfo()

