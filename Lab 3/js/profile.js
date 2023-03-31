"use strict"

class DonationScoreboardViewModel {
    constructor(name, amount) {
        this.name = name;
        this.amount = amount;
    }
}

let mockDonations = [new DonationScoreboardViewModel("Donator #1", 200),
                    new DonationScoreboardViewModel("Donator #2", 1),
                    new DonationScoreboardViewModel("Donator #3", 12),
                    new DonationScoreboardViewModel("Donator #4", 2),
                    new DonationScoreboardViewModel("Donator #1", 10),
                    new DonationScoreboardViewModel("Donator #5", 15),
                    new DonationScoreboardViewModel("Donator #2", 6),
                    new DonationScoreboardViewModel("Donator #12", 0)];

function distinctAndSort() {
    const resultArray = Object.values(mockDonations.reduce((acc, {name, amount}) => {
        acc[name] = acc[name] || { name, amount: 0 };
        acc[name].amount += amount;
        return acc;
    }, {}))
        .sort((a, b) => b.amount - a.amount);

    return resultArray;
}

function fillScoreboard() {
    let scoreboardDiv = document.querySelector(".scoreboard")
    let data = distinctAndSort().slice(0, 5)
    for (let i = 0; i < data.length; i++) {
        let row = document.createElement("div");
        row.className = ("scoreboard__row")
        if(i == 0) {
            row.classList.add("scoreboard-top-donator")
        }
        else if (i % 2 == 0) {
            row.classList.add("scoreboard-row-primary")
        }
        else {
            row.classList.add("scoreboard-row-secondary")
        }

        let row_pos = document.createElement("div");
        row_pos.className = "scoreboard__row-position"
        if (i == 0) {
            let pos = document.createElement("img");
            pos.setAttribute("src", "../images/heart.png")
            row_pos.appendChild(pos)
        }
        else {
            row_pos.innerHTML = `${i}.`
        }

        row.appendChild(row_pos);

        let row_donator = document.createElement("div")
        row_donator.className = "scoreboard__row-donator-name"
        row_donator.innerHTML = `${data[i].name}`;

        row.appendChild(row_donator);

        let row_amount = document.createElement("div")
        row_amount.className = "scoreboard__row-amount"
        row_amount.innerHTML = `\$${data[i].amount}`;

        row.appendChild(row_amount);

        scoreboardDiv.appendChild(row)
    }
}

function adjustProgress() {
    let totalCount = distinctAndSort().reduce((acc, curr) => acc + curr.amount, 0)
    let percentage = `${Math.round(totalCount / 410 * 100)}%`;
    let progressReached = document.querySelector(".progress-reached");
    progressReached.style.width = percentage;
    progressReached.innerHTML = percentage;
}

fillScoreboard()
adjustProgress()
console.log(distinctAndSort())