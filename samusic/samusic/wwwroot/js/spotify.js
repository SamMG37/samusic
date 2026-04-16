window.onload = function () {
    const button = document.getElementById("searchButton");
    const searchBox = document.getElementById("searchBox");

    if (button) {
        button.addEventListener("click", searchMusic);
    }

    if (searchBox) {
        searchBox.addEventListener("keydown", function (event) {
            if (event.key === "Enter") {
                event.preventDefault();
                searchMusic();
            }
        });
    }

    loadGenreRow("pop", "genre-pop");
    loadGenreRow("hip-hop", "genre-hip-hop");
    loadGenreRow("metal", "genre-metal");
    loadGenreRow("rock", "genre-rock");
};

async function searchMusic() {
    const query = document.getElementById("searchBox").value.trim();

    if (!query) return;

    try {
        const response = await fetch("/Music/Search?query=" + encodeURIComponent(query));

        if (!response.ok) {
            const text = await response.text();
            console.error("Search request failed:", text);
            document.getElementById("searchSection").style.display = "block";
            document.getElementById("results").innerHTML = "<p>Search could not be completed.</p>";
            return;
        }

        const data = await response.json();

        if (data.tracks && data.tracks.items && data.tracks.items.length > 0) {
            document.getElementById("searchSection").style.display = "block";
            displayTracks(data.tracks.items);
            saveRecentSearches(data.tracks.items);
            document.getElementById("searchSection").scrollIntoView({ behavior: "smooth", block: "start" });
        } else {
            document.getElementById("searchSection").style.display = "block";
            document.getElementById("results").innerHTML = "<p>No search results found.</p>";
        }
    } catch (error) {
        console.error("Search failed:", error);
        document.getElementById("searchSection").style.display = "block";
        document.getElementById("results").innerHTML = "<p>Search could not be completed.</p>";
    }
}

async function loadGenreRow(genre, containerId) {
    try {
        const response = await fetch("/Music/Genre?genre=" + encodeURIComponent(genre));

        if (!response.ok) {
            const text = await response.text();
            console.error("Genre request failed:", text);
            return;
        }

        const data = await response.json();

        if (data.tracks && data.tracks.items) {
            displayGenreAlbums(data.tracks.items, containerId);
        }
    } catch (error) {
        console.error("Genre load failed:", error);
    }
}

function displayTracks(tracks) {
    const results = document.getElementById("results");
    results.innerHTML = "";

    tracks.forEach(track => {
        const card = document.createElement("div");
        card.className = "musicCard searchCard";

        const imageUrl = track.album?.images?.length > 0 ? track.album.images[0].url : "";
        const artistNames = track.artists?.map(a => a.name).join(", ") ?? "Unknown artist";

        card.innerHTML = `
            <img src="${imageUrl}" alt="${track.name}">
            <h4>${track.name}</h4>
            <p>${artistNames}</p>
        `;

        card.addEventListener("click", function () {
            showDetails(track);
        });

        results.appendChild(card);
    });
}

function displayGenreAlbums(tracks, containerId) {
    const container = document.getElementById(containerId);
    container.innerHTML = "";

    const seenAlbums = new Set();

    tracks.forEach(track => {
        if (!track.album || !track.album.id || seenAlbums.has(track.album.id)) {
            return;
        }

        seenAlbums.add(track.album.id);

        const card = document.createElement("div");
        card.className = "musicCard carouselCard";

        const imageUrl = track.album?.images?.length > 0 ? track.album.images[0].url : "";
        const artistNames = track.artists?.map(a => a.name).join(", ") ?? "Unknown artist";

        card.innerHTML = `
            <img src="${imageUrl}" alt="${track.album.name}">
            <h4>${track.album.name}</h4>
            <p>${artistNames}</p>
        `;

        card.addEventListener("click", function () {
            showDetails(track);
        });

        container.appendChild(card);
    });
}

function showDetails(track) {
    const imageUrl = track.album?.images?.length > 0 ? track.album.images[0].url : "";
    const artistNames = track.artists?.map(a => a.name).join(", ") ?? "Unknown artist";
    const albumName = track.album?.name ?? "Unknown album";
    const releaseDate = track.album?.release_date ?? "Unknown";
    const spotifyUrl = track.external_urls?.spotify ?? "#";


    document.getElementById("detailsSection").style.display = "block";

    document.getElementById("details").innerHTML = `
        <div class="detailsCard">
            <h2>${track.name}</h2>
            <img src="${imageUrl}" alt="${track.name}">
            <p><strong>Artist:</strong> ${artistNames}</p>
            <p><strong>Album:</strong> ${albumName}</p>
            <p><strong>Release date:</strong> ${releaseDate}</p>
            <p><a href="${spotifyUrl}" target="_blank">Open in Spotify</a></p>

            <button type="button" class="saveButton" onclick="saveFavourite(${JSON.stringify(track).replace(/"/g, '&quot;')})">
                Save to favourites
            </button>

            <button type="button" class="ratingsButton" onclick="window.location.href='/Music/Reviews?trackId=${track.id}'">
                    View ratings
            </button>
        </div>
    `;

    document.getElementById("detailsSection").scrollIntoView({ behavior: "smooth", block: "start" });
}


async function saveFavourite(track) {
    const favourite = {
        spotifyTrackId: track.id,
        trackName: track.name,
        artistNames: track.artists?.map(a => a.name).join(", ") ?? "Unknown artist",
        albumName: track.album?.name ?? "",
        albumImageUrl: track.album?.images?.length > 0 ? track.album.images[0].url : "",
        spotifyUrl: track.external_urls?.spotify ?? "",
        releaseDate: track.album?.release_date ?? ""
    };

    try {
        const response = await fetch("/Music/SaveFavourite", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(favourite)
        });

        if (response.redirected || response.status === 401 || response.status === 403) {
            showToast("Log in to favourite.", "error");
            return;
        }

        const contentType = response.headers.get("content-type");

        if (!contentType || !contentType.includes("application/json")) {
            showToast("Log in to favourite.", "error");
            return;
        }

        const data = await response.json();
        showToast(data.message, data.success ? "success" : "error");
    } catch (error) {
        console.error("Save favourite failed:", error);
        showToast("Log in to favourite.", "error");
    }
}

function saveRecentSearches(tracks) {
    if (!tracks || tracks.length === 0) return;

    const mappedTracks = tracks.slice(0, 8).map(track => ({
        spotifyTrackId: track.id,
        trackName: track.name,
        artistNames: track.artists?.map(a => a.name).join(", ") ?? "Unknown artist",
        albumImageUrl: track.album?.images?.length > 0 ? track.album.images[0].url : "",
        spotifyUrl: track.external_urls?.spotify ?? ""
    }));

    let recentSearchedTracks = [];

    try {
        recentSearchedTracks = JSON.parse(localStorage.getItem("recentSearchedTracks")) || [];
    } catch {
        recentSearchedTracks = [];
    }

    mappedTracks.reverse().forEach(track => {
        recentSearchedTracks = recentSearchedTracks.filter(t => t.spotifyTrackId !== track.spotifyTrackId);
        recentSearchedTracks.unshift(track);
    });

    if (recentSearchedTracks.length > 8) {
        recentSearchedTracks = recentSearchedTracks.slice(0, 8);
    }

    localStorage.setItem("recentSearchedTracks", JSON.stringify(recentSearchedTracks));
}

function showToast(message, type = "success") {
    const toast = document.getElementById("toastMessage");

    if (!toast) return;

    toast.textContent = message;
    toast.className = `toastMessage show ${type}`;

    setTimeout(() => {
        toast.className = "toastMessage";
    }, 2500);
}