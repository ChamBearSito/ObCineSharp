// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const thefunction = (i) => {
    let elAsi = document.getElementById(`a_${i}`);
    if (elAsi.style.color == black) {
        elAsi.style.color = green;
        elAsi.style.borderColor = green;
    } else {
        elAsi.style.color = black;
        elAsi.style.borderColor = black;
    }
}
