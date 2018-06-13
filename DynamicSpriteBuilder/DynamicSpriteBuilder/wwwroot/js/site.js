const button = document.querySelector('button');
const spriteContainer = document.querySelector('.sprites');
const label = document.querySelector('label');
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
            let result = JSON.parse(xhr.response);
            label.innerHTML = `Generated ${result.key}.png...`;
            let img = document.createElement('img');
            img.src = result.value;
            spriteContainer.prepend(img);
        } else {
            console.error(new Error(xhr.statusText));
        }
    };
    xhr.onerror = () => {
        reject(new Error(xhr.statusText));
    };
    xhr.send();
});
