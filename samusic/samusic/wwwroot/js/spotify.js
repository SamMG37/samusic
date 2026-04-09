window.onload = function () {
    loadTrending();
}

window.searchMusic = async function () {
    let query = document.getElementById("searchBox").value;
    let response = await fetch("/Music/Search?query=" + query);
    let data = await response.json();
    let tracks = JSON.parse(data);
    displayTracks(tracks.tracks.items);
}

window.loadTrending = async function () {
    let response = await fetch("/Music/Trending");
    let data = await response.json();
    let albums = JSON.parse(data);
    displayTrending(albums.albums.items);
}

async function searchMusic() {

    let query =
        document.getElementById(
            "searchBox"
        ).value;

    let response = await fetch(
            "/Music/Search?query=" + query
        );

    let text = await response.text();

    let tracks = JSON.parse(text);

    displayTracks(tracks.tracks.items
    );

}

function displayTrending(albums) {

    let results =
        document.getElementById(
            "results"
        );

    results.innerHTML =
        "<h2>Trending Music</h2>";

    albums.forEach(album => {

        let card =
            document.createElement("div");

        card.style.border =
            "1px solid grey";

        card.style.padding =
            "10px";

        card.style.margin =
            "10px";

        card.style.width =
            "200px";

        card.style.display =
            "inline-block";

        card.innerHTML = `<img src="${album.images[0].url}"width="150">
        <h3>${album.name}</h3>
            <p>${album.artists[0].name}</p>
        `;

        results.appendChild(card);

    });

}

function showDetails(track) {

    document.getElementById("details").innerHTML = <><h2>${track.name}</h2><img src="${track.album.images[0].url}" width="250">
        <p>Artist:${track.artists[0].name}</p>
        <p>Album:${track.album.name}</p>
        <p>Release:${track.album.release_date}</p>
        <p>Popularity:${track.popularity}</p>
        <a href="${track.external_urls.spotify}" target="_blank">Open Spotify</a>`;
    }; </></>
}