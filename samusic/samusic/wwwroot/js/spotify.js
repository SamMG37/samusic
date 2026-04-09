window.onload = function () {
    loadTrending();
};

async function searchMusic() {

    let query = document.getElementById("searchBox").value;

    let response = await fetch("/Music/Search?query=" + query);

    let data = await response.json();

    console.log(data);

    displayTracks(data.tracks.items);

    if (data.tracks && data.tracks.items) {
        displayTracks(data.tracks.items);
    }
    else {
        console.log("Spotify returned:", data);
    }

}

async function loadTrending() {

    let response = await fetch("/Music/Trending");

    let data = await response.json();

    console.log(data);

    displayTrending(data.albums.items);

    if (data.albums && data.albums.items) {
        displayTrending(data.albums.items);
    }
    else {
        console.log("Spotify returned:", data);
    }

}

function displayTracks(tracks) {

    let results =
        document.getElementById("results");

    results.innerHTML = "<h2>Search Results</h2>";

    tracks.forEach(track => {

        let card = document.createElement("div");

        card.className = "musicCard";

        card.innerHTML = `<img src="${track.album.images[0].url}" width="150">
        <h3>${track.name}</h3>
        <p>${track.artists[0].name}</p>`;

        card.onclick = function ()
        {
            showDetails(track);
        };

        results.appendChild(card);

    });

}

function displayTrending(albums) {

    let results = document.getElementById("results");

    results.innerHTML = "<h2>Trending Music</h2>";

    albums.forEach(album => {

        let card = document.createElement("div");

        card.className = "musicCard";

        card.innerHTML = `<img src="${album.images[0].url}" width="150">
        <h3>${album.name}</h3>
        <p>${album.artists[0].name}</p>`;

        results.appendChild(card);

    });

}

function showDetails(track) {

    document.getElementById("details").innerHTML = `<h2>${track.name}</h2>
        <img src="${track.album.images[0].url}" width="250">

        <p><b>Artist:</b>${track.artists[0].name}</p>

        <p><b>Album:</b>${track.album.name}</p>

        <p><b>Release:</b>${track.album.release_date}</p>

        <p><b>Popularity:</b>${track.popularity}</p>

        <a href="${track.external_urls.spotify}"target="_blank">Open in Spotify</a>`;

}