"use strict"

class ProfileCardViewModel {
    constructor(id, title, reached, author, category) {
        this.id = id;
        this.title = title;
        this.reached = reached;
        this.author = author;
        this.category = category;
    }
}

let profileObjects = [new ProfileCardViewModel(0, "This is a startup", 25, "medadon", "Start-ups"),
                    new ProfileCardViewModel(0, "This is a startup", 31, "medadon", "Start-ups"),
                    new ProfileCardViewModel(0, "This is a charity", 22, "medadon", "Charity"),
                    new ProfileCardViewModel(0, "This is a charity", 41, "medadon", "Charity"),
                    new ProfileCardViewModel(0, "This is a content", 12, "medadon", "Content makers"),
                    new ProfileCardViewModel(0, "This is a content", 3, "medadon", "Content makers"),
                    new ProfileCardViewModel(0, "This is a streamer", 5, "medadon", "Streaming"),
                    new ProfileCardViewModel(0, "This is a streamer", 54, "medadon", "Streaming"),
                    new ProfileCardViewModel(0, "This is a streamer", 98, "medadon", "Steaming"),
                    new ProfileCardViewModel(0, "This is a development", 67, "medadon", "Development"),
                    new ProfileCardViewModel(0, "This is a development", 78, "medadon", "Development")]


let currentPage = 0;
let currentCategory = "All";

async function getProfilesCards() {
    const response = await fetch('http://localhost:5043/api/Profile', {
        method: "GET",
        headers: { "Accept": "application/json" }
    });

    let cards;
    if (response.ok === true) {
        cards = await response.json();
    }

    profileObjects = cards

    setDropdownListeners()
    drawPager()
    fillCards(0, 8, currentCategory)
}



function fillCards(from, to, category) {
    console.log(currentCategory)
    let cards = document.querySelector(".cards-container")
    if (category == "All")
        var spliced = profileObjects.slice()
    else
        var spliced = profileObjects.filter(obj => {
            return obj.category === currentCategory;
        });
    spliced.splice(from, to).forEach(item => {
        let card = document.createElement("div")
        card.className = "card"

        let card_image = document.createElement("div")
        card_image.className = "card__image"
        card_image.style.backgroundImage=`url(${item.imageUrl})`
        card_image.style.backgroundSize='cover'
        card.appendChild(card_image)

        let card_info = document.createElement("div")
        card_info.className = "card__information"

        let title = document.createElement("p")
        title.innerHTML = item.title

        card_info.appendChild(title)

        let progressBar = document.createElement("div")
        progressBar.className = "card__progress-bar"

        let progressReached = document.createElement("div")
        progressReached.className = "card__progress-reached"
        progressReached.style.width = `${item.reached}%`

        progressBar.appendChild(progressReached)
        card_info.appendChild(progressBar)

        let reachedText = document.createElement("b")
        reachedText.innerHTML = `${item.reached}% reached`

        card_info.appendChild(reachedText)

        let author = document.createElement("div")
        author.className = "card__author"

        let authorName = document.createElement("p")
        authorName.innerHTML = `by <b>${item.author}</b>`

        let hiddenLink = document.createElement("a")
        hiddenLink.className = "card__link"
        hiddenLink.href=`profile.html?id=${item.id}`

        author.appendChild(authorName)

        card_info.appendChild(author)

        card.appendChild(card_info)

        card.appendChild(hiddenLink)

        cards.appendChild(card)
    })
}

function removeCards() {
    let cards = document.querySelector(".cards-container")
    let allCards = document.querySelectorAll(".card")
    allCards.forEach(item => {
        cards.removeChild(item)
    })
}

function drawPager() {
    let pager = document.querySelector(".pager")
    let pagesCount = Math.ceil(profileObjects.length / 8)
    console.log(profileObjects)

    for (let i = 0; i < pagesCount; i++) {
        let pagerElem = document.createElement("button")
        pagerElem.className = "pager-element"

        if (currentPage == i) {
            pagerElem.classList.add("pager-element__active")
        }

        pagerElem.innerHTML = `${i + 1}`
        pager.appendChild(pagerElem)

        pagerElem.addEventListener('click', function (event) {
            currentPage = i;
            removeCards()
            fillCards(currentPage*8, (currentPage+1)*8, currentCategory)
            let currentActive = document.querySelector(".pager-element__active")
            currentActive.classList.remove("pager-element__active")
            pagerElem.classList.add("pager-element__active")
        })
    }
}

function setDropdownListeners() {
    let dropdown = document.querySelector(".dropdown-menu")
    let elems = dropdown.querySelectorAll(".dropdown-item")
    elems.forEach(item => {
        item.addEventListener('click', function(event) {
           currentCategory = item.innerHTML
            removeCards()
            fillCards(0, 8, currentCategory)
        });
    })

}

getProfilesCards();