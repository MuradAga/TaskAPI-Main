getMovies();
getCategories();
getTrendMovies();

async function getMovies() {
  let response = await fetch("https://localhost:7293/api/Movies", {
    method: "GET", // *GET, POST, PUT, DELETE, etc.
    mode: "cors", // no-cors, *cors, same-origin
    cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
    headers: {
      "Content-Type": "application/json",
    },
  });
  const data = response.json();
  data.then((movies) =>
    movies.forEach((movie) => {
      let categories = "";
      movie.categories.forEach((category) => {
        categories += category.name + ", ";
      });
      document.getElementsByClassName("list")[0].innerHTML += `<li class="film">
        <a
          class="tt"
          href="https://www.fullhdfilmizlesene.de/film/buyuk-felaket-asit-yagmuru-acide/"
          >${movie.name} izle</a>
        <h2 class="film-tt">
          <span class="film-title">${movie.name}</span>
        </h2>

        <span class="imdb">${movie.imdbPoint}</span>
        <span class="yrm">0 yorum</span>
        <span class="film-yil">${movie.year}</span>
        <span class="ktt">${categories}</span>
        <img
          src="${movie.coverUrl}"
          data-src="${movie.coverUrl}"
          data-srcset="${movie.coverUrl} 1x, ${movie.coverUrl} 1.5x"
          width="215"
          height="327"
          alt="${movie.name} izle"
          class="afis ls-is-cached lazyloaded"
          srcset="
          ${movie.coverUrl} 1x,
          ${movie.coverUrl} 1.5x
          "
        />
      </li>`;
    })
  );
}

async function getCategories() {
  let response = await fetch("https://localhost:7293/api/Categories", {
    method: "GET", // *GET, POST, PUT, DELETE, etc.
    mode: "cors", // no-cors, *cors, same-origin
    cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
    headers: {
      "Content-Type": "application/json",
    },
  });
  const data = response.json();
  data.then((categories) =>
  categories.forEach((category) => {      
    document.getElementById("categories").innerHTML += `<option value="${category.id}">${category.name}</option>`;

      document.getElementsByClassName("turler")[0].innerHTML += `<li>
      <a href="https://www.fullhdfilmizlesene.de/filmizle/aile-filmleri-izle">${category.name}</a>
    </li>`;
    })
  );
}

async function getTrendMovies() {
  let response = await fetch("https://localhost:7293/api/Movies/getTrendMovies", {
    method: "GET", // *GET, POST, PUT, DELETE, etc.
    mode: "cors", // no-cors, *cors, same-origin
    cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
    headers: {
      "Content-Type": "application/json",
    },
  });
  const data = response.json();
  data.then((movies) =>
  movies.forEach((movie) => {     
      document.getElementsByClassName("nette-ilk")[0].innerHTML += `<li>
      <a
        href="https://www.fullhdfilmizlesene.de/film/hizli-ve-ofkeli-9/"
      >
        <img
          src="${movie.coverUrl}"
          width="48"
          height="48"
          alt="${movie.name}"
        />
        <div class="ni-dty">
          <div class="ni-tt">
          ${movie.name}
          </div>
        </div>
      </a>
    </li>`;
    })
  );
}

async function filterMovies() {
  let categoryId = document.getElementById("categories").value;
  let year = document.getElementById("years").value;
  let imdbPoint = document.getElementById("imdbs").value;

  let response = await fetch(`https://localhost:7293/api/Movies/getMoviesByFilter?categoryId=${categoryId}&year=${year}&imdbPoint=${imdbPoint}`, {
    method: "GET", // *GET, POST, PUT, DELETE, etc.
    mode: "cors", // no-cors, *cors, same-origin
    cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
    headers: {
      "Content-Type": "application/json",
    },
  });
  document.getElementsByClassName("list")[0].innerHTML = "";
  const data = response.json();
  data.then((movies) =>
    movies.forEach((movie) => {
      let categories = "";
      movie.categories.forEach((category) => {
        categories += category.name + ", ";
      });
      document.getElementsByClassName("list")[0].innerHTML += `<li class="film">
        <a
          class="tt"
          href="https://www.fullhdfilmizlesene.de/film/buyuk-felaket-asit-yagmuru-acide/"
          >${movie.name} izle</a>
        <h2 class="film-tt">
          <span class="film-title">${movie.name}</span>
        </h2>

        <span class="imdb">${movie.imdbPoint}</span>
        <span class="yrm">0 yorum</span>
        <span class="film-yil">${movie.year}</span>
        <span class="ktt">${categories}</span>
        <img
          src="${movie.coverUrl}"
          data-src="${movie.coverUrl}"
          data-srcset="${movie.coverUrl} 1x, ${movie.coverUrl} 1.5x"
          width="215"
          height="327"
          alt="${movie.name} izle"
          class="afis ls-is-cached lazyloaded"
          srcset="
          ${movie.coverUrl} 1x,
          ${movie.coverUrl} 1.5x
          "
        />
      </li>`;
    })
  );
}