document.addEventListener("DOMContentLoaded", function () {
    loadRecentSearchedTracks();

    const clearButton = document.getElementById("clearRecentButton");

    if (clearButton) {
        clearButton.addEventListener("click", clearRecentSearchedTracks);
    }
});

function loadRecentSearchedTracks() {
    const section = document.getElementById("recentSection");
    const container = document.getElementById("recentTracks");
    const clearButton = document.getElementById("clearRecentButton");

    if (!section || !container) return;

    const stored = localStorage.getItem("recentSearchedTracks");

    if (!stored) {
        section.style.display = "none";
        return;
    }

    let tracks = [];

    try {
        tracks = JSON.parse(stored);
    } catch {
        section.style.display = "none";
        return;
    }

    if (!tracks || tracks.length === 0) {
        section.style.display = "none";
        return;
    }

    section.style.display = "block";
    container.innerHTML = "";

    if (clearButton) {
        clearButton.style.display = "inline-block";
    }

    tracks.forEach(track => {
        const card = document.createElement("div");
        card.className = "musicCard searchCard";

        const imageUrl = track.albumImageUrl ?? "";
        const artistNames = track.artistNames ?? "Unknown artist";
        const spotifyUrl = track.spotifyUrl ?? "#";

        card.innerHTML = `
            <img src="${imageUrl}" alt="${track.trackName}">
            <h4>${track.trackName}</h4>
            <p>${artistNames}</p>
        `;

        card.addEventListener("click", function () {
            window.open(spotifyUrl, "_blank");
        });

        container.appendChild(card);
    });
}

function clearRecentSearchedTracks() {
    localStorage.removeItem("recentSearchedTracks");

    const section = document.getElementById("recentSection");
    const container = document.getElementById("recentTracks");

    if (container) {
        container.innerHTML = "";
    }

    if (section) {
        section.style.display = "none";
    }
}