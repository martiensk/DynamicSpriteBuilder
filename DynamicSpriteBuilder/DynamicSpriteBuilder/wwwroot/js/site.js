const button = document.querySelector('button');
const spriteContainer = document.querySelector('.sprites');
const httpStatusRange = {
    start: 200,
    end: 300
};

button.addEventListener('click', (e) => {
    e.preventDefault();
    const xhr = new XMLHttpRequest();
    xhr.open('GET', '/?handler=spritesheet');
    xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
    xhr.onload = () => {
        if (xhr.status >= httpStatusRange.start && xhr.status < httpStatusRange.end) {
            console.log(xhr.response);
        } else {
            console.error(new Error(xhr.statusText));
        }
    };
    xhr.onerror = () => {
        reject(new Error(xhr.statusText));
    };
    xhr.send();
});
