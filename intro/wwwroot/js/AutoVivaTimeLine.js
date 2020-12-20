

class AutoVivaTimeLine{
    constructor(rootPath){
        this.rootPath = rootPath;
    }
    static months(){ return ["Jan.", "Feb.", "Mar.", "Apr.", "May.", "Jun.", "Jul.", "Aug.", "Sep.", "Oct.", "Nov.", "Dec."] };
    getYearAndMonth(date){
        var year = date.getFullYear();
        var month = AutoVivaTimeLine.months()[date.getMonth()];

        return `<dt>${month} ${year}</dt>`;
    }
    build(posts){
        var ret = [];
        var month = -1;
        var year = -1;
        var l = false;
        posts.forEach(post => {
            var time = new Date(post.Time);
            if(time.getFullYear() != year || time.getMonth() != month){
                ret.push(this.getYearAndMonth(time));
                year = time.getFullYear();
                month = time.getMonth();
                l = !l;
            }

            var post = this.getPost(post, l ? 'left' : 'right');
            ret.push(post);
        });
        return ret;
    }
    getPost(post, position){
        if(!position)
            position = 'left';
        post.Url = this.convertRootPath(post.Url);
        post.Image = this.convertRootPath(post.Image);

        var dd = document.createElement("dd");
        dd.className = `pos-${position} clearfix`;
        var temp = document.createElement("div");
        temp.className = "circ";
        dd.appendChild(temp);
        temp = document.createElement("div");
        temp.className = "time";
        var time = new Date(post.Time);
        var month = AutoVivaTimeLine.months()[time.getMonth()];
        var day = time.getDate().toString();
        if(day.length == 1) day = "0" + day;
        temp.innerText = `${month} ${day}`;
        dd.appendChild(temp);

        var event = document.createElement("div");

        event.className = "events";
        temp = document.createElement("div");
        temp.className = "events-header";
        temp.innerText = post.Subject;
        event.appendChild(temp);

        var body = document.createElement('div');
        body.className = "events-body";
        body.style.padding = "0.3em";
        var card = document.createElement("div");
        card.className = "card";
        var img = document.createElement("img");
        img.src = post.Image;
        img.className = "card-img-top";
        img.alt = "壞了50收";
        card.appendChild(img);
        var cardBody = document.createElement("div");
        cardBody.className = "card-body";
        var title = document.createElement("h5");
        title.className = "card-title";
        title.innerText = post.Subject;
        cardBody.appendChild(title);
        var content = document.createElement("p");
        content.className = "card-text";
        content.innerText = post.Content;
        cardBody.appendChild(content);
        var more = document.createElement("a");
        more.href = post.Url;
        more.className = "btn btn-primary";
        more.innerText = "More Infomation.."
        cardBody.appendChild(more);


        card.appendChild(cardBody);

        body.appendChild(card);

        event.appendChild(body);

        var footer = document.createElement("div");
        footer.className = "events-footer";
        temp = document.createElement('a');
        temp.href = "#";
        temp.innerText = "　";
        footer.appendChild(temp);
        event.appendChild(footer);

        dd.appendChild(event);
        return dd;
    }

    convertRootPath(path){
        if(path.startsWith("~/")){
            path = path.replace('~/', '');
            path = `${this.rootPath}/${path}`;
        
        }
        return path;
    }
    
}