document.addEventListener("DOMContentLoaded", function () {
    loadRecentTracks();
});

function loadRecentTracks() {
    const section = document.getElementById("recentSection");
    const container = document.getElementById("recentTracks");

    if (!section || !container) return;

    const stored = localStorage.getItem("recentTracks");

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