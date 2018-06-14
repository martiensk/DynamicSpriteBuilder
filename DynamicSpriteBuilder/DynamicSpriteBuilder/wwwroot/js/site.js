const button = document.querySelector('button');
const spriteContainer = document.querySelector('.sprites');
const label = document.querySelector('label');
const head = document.getElementsByTagName('head')[0];
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
            spriteContainer.innerHTML = "";
            // Add CSS
            let s = document.createElement('style');
            s.setAttribute('type', 'text/css');
            if (s.styleSheet) {
                s.styleSheet.cssText = result.css;
            } else {
                s.appendChild(document.createTextNode(result.css));
            }
            head.appendChild(s);
            // Show name of generated spritesheet
            label.innerHTML = `Generated ${result.name}.png...`;
            // Display spritesheet to user
            let img = document.createElement('img');
            img.src = result.path;
            spriteContainer.append(img);
            // Display individual sprites to user.
            for (var sprite of result.sprites) {
                let s_img = document.createElement('img');
                s_img.classList.add(result.name);
                s_img.classList.add(sprite);
                spriteContainer.append(s_img);
            }
        } else {
            console.error(new Error(xhr.statusText));
        }
    };
    xhr.onerror = () => {
        reject(new Error(xhr.statusText));
    };
    xhr.send();
});
