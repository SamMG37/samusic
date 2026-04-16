function selectFavouriteCard(card) {
    const section = document.getElementById("selectedFavouriteSection");
    const details = document.getElementById("selectedFavouriteDetails");

    if (!section || !details) return;

    const trackName = card.dataset.trackName ?? "";
    const artistNames = card.dataset.artistNames ?? "";
    const albumName = card.dataset.albumName ?? "";
    const albumImageUrl = card.dataset.albumImageUrl ?? "";
    const spotifyUrl = card.dataset.spotifyUrl ?? "#";
    const releaseDate = card.dataset.releaseDate ?? "";
    const spotifyTrackId = card.dataset.spotifyTrackId ?? "";

    details.innerHTML = `
        <div class="detailsCard">
            <h2>${trackName}</h2>
            <img src="${albumImageUrl}" alt="${trackName}">
            <p><strong>Artist:</strong> ${artistNames}</p>
            <p><strong>Album:</strong> ${albumName}</p>
            <p><strong>Release date:</strong> ${releaseDate}</p>

            <div class="detailsButtonRow">
                <a href="${spotifyUrl}" target="_blank" class="ratingsButton detailsLinkButton">
                    Open in Spotify
                </a>

                <a href="/Music/Reviews?trackId=${spotifyTrackId}" class="ratingsButton detailsLinkButton">
                    View ratings
                </a>
            </div>
        </div>
    `;

    section.style.display = "block";
    section.scrollIntoView({ behavior: "smooth", block: "start" });
}