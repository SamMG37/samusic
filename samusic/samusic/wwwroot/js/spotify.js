window.onload = function () {
    const button = document.getElementById("searchButton");

    if (button) {
        button.addEventListener("click", searchMusic);
    }

    loadTrending();
};

async function searchMusic() {
    const query = document.getElementById("searchBox").value.trim();

    if (!query) {
        return;
    }

    try {
        const response = await fetch("/Music/Search?query=" + encodeURIComponent(query));

        if (!response.ok) {
            const text = await response.text();
            console.error("Search request failed:", text);
            document.getElementById("results").innerHTML = "<h2>Search could not be completed.</h2>";
            return;
        }

        const data = await response.json();
        console.log("Search response:", JSON.stringify(data, null, 2));

        if (data.tracks && data.tracks.items && data.tracks.items.length > 0) {
            displayTracks(data.tracks.items, "Search Results");
        } else {
            document.getElementById("results").innerHTML = "<h2>No search results found.</h2>";
        }
    } catch (error) {
        console.error("Search failed:", error);
        document.getElementById("results").innerHTML = "<h2>Search could not be completed.</h2>";
    }
}

async function loadTrending() {
    try {
        const response = await fetch("/Music/Trending");

        if (!response.ok) {
            const text = await response.text();
            console.error("Trending request failed:", text);
            document.getElementById("results").innerHTML = "<h2>Trending music could not be loaded.</h2>";
            return;
        }

        const data = await response.json();
        console.log("Trending response:", JSON.stringify(data, null, 2));

        if (data.tracks && data.tracks.items && data.tracks.items.length > 0) {
            displayTracks(data.tracks.items, "Trending Music");
        } else {
            document.getElementById("results").innerHTML = "<h2>Trending music could not be loaded.</h2>";
        }
    } catch (error) {
        console.error("Trending failed:", error);
        document.getElementById("results").innerHTML = "<h2>Trending music could not be loaded.</h2>";
    }
}

function displayTracks(tracks, headingText) {
    const results = document.getElementById("results");
    results.innerHTML = `<h2>${headingText}</h2>`;

    tracks.forEach(track => {
        const card = document.createElement("div");
        card.className = "musicCard";

        const imageUrl = track.album?.images?.length > 0 ? track.album.images[0].url : "";

        card.innerHTML = `
            <img src="${imageUrl}" width="150">
            <h3>${track.name}</h3>
            <p>${track.artists.map(a => a.name).join(", ")}</p>
        `;

        card.addEventListener("click", function () {
            showDetails(track);
        });

        results.appendChild(card);
    });
}

function showDetails(track) {
    const imageUrl = track.album?.images?.length > 0 ? track.album.images[0].url : "";

    document.getElementById("details").innerHTML = `
        <h2>${track.name}</h2>
        <img src="${imageUrl}" width="250">
        <p><b>Artist:</b> ${track.artists.map(a => a.name).join(", ")}</p>
        <p><b>Album:</b> ${track.album.name}</p>
        <p><b>Release date:</b> ${track.album.release_date}</p>
        <p><b>Popularity:</b> ${track.popularity}</p>
        <a href="${track.external_urls.spotify}" target="_blank">Open in Spotify</a>
    `;
}