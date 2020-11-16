
Date.prototype.format = function (fmt) {
    var o = {
      "M+": this.getMonth() + 1,
      "d+": this.getDate(),
      "h+": this.getHours(),
      "m+": this.getMinutes(), 
      "s+": this.getSeconds(), 
      "q+": Math.floor((this.getMonth() + 3) / 3),
      "S": this.getMilliseconds() 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
    if (new RegExp("(" +  k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" +  o[k]).substr(("" + o[k]).length)));
    return fmt;
  }
  
  Date.prototype.addSeconds = function(seconds) {
    this.setSeconds(this.getSeconds() + seconds);
    return this;
  }
  
  Date.prototype.addMinutes = function(minutes) {
    this.setMinutes(this.getMinutes() + minutes);
    return this;
  }
  
  Date.prototype.addHours = function(hours) {
    this.setHours(this.getHours() + hours);
    return this;
  }
  
  Date.prototype.addDays = function(days) {
    this.setDate(this.getDate() + days);
    return this;
  }
  
  Date.prototype.addMonths = function(months) {
    this.setMonth(this.getMonth() + months);
    return this;
  }
  
  Date.prototype.addYears = function(years) {
    this.setFullYear(this.getFullYear() + years);
    return this;
  }
  
  function diffSeconds(milliseconds) {
    return Math.floor(milliseconds / 1000);
  }
  
  function diffMinutes(milliseconds) {
    return Math.floor(milliseconds / 1000 / 60);
  }
  
  function diffHours(milliseconds) {
    return Math.floor(milliseconds / 1000 / 60 / 60);
  }
  
  function diffDays(milliseconds) {
    return Math.floor(milliseconds / 1000 / 60 / 60 / 24);
  }