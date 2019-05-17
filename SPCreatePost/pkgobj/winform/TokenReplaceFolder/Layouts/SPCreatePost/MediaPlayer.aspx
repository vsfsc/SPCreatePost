<html xmlns:mso="urn:schemas-microsoft-com:office:office" xmlns:msdt="uuid:C2F41010-65B3-11d1-A29F-00AA00C14882">
  
<head>
    <title>SilverLight Media Player Sample Page</title>
    <script type="text/javascript" src="/_layouts/mediaplayer.js"></script>

    <script type="text/javascript">

        //Gets the media player.
        function getMediaPlayer() {
            var p = document.getElementById("MediaPlayerHost")
            var obj = p.getElementsByTagName("object");
            return obj[0].Content.MediaPlayer;
        }

        //Initialize the media player object and set values for its properties. Customize MediaUrlField and PreviewURLField values for your local environment.
        function init() {
            var serverStr = window.location.href;
            serverStr = serverStr.substr(7);
            serverStr = serverStr.substr(0, serverStr.indexOf("/"));

            document.getElementById("MediaURLField").value = "http://va.neu.edu.cn/courses/mysitedesign/doclib/第一章/1.mp4";
            document.getElementById("PreviewURLField").value = "http://xqx2012/_layouts/15/SPCreatePost/shipin.jpg";
            document.getElementById("TitleField").value = "API Test Page";
            document.getElementById("TemplateURLField").value = "http://" + serverStr + "/_layouts/15/STYLES/AlternateMediaPlayer.xaml";
        }

        //Set properties of the media player, including media URL, preview image URL, template URL, title, autoplay, whether to repeat, and default display mode. 
        function SetMediaSource() {
            var elm = document.getElementById("MediaURLField");
            var p = getMediaPlayer();
            p.MediaSource = elm.value;
        }
        function SetPreviewImageSource() {
            var elm = document.getElementById("PreviewURLField");
            var p = getMediaPlayer();
            p.PreviewImageSource = elm.value;
        }
        function SetMediaTitle() {
            var elm = document.getElementById("TitleField");
            var p = getMediaPlayer();
            p.MediaTitle = elm.value;
        }
        function SetTemplateSource() {
            var elm = document.getElementById("TemplateURLField");
            var p = getMediaPlayer();
            p.TemplateSource = elm.value;
        }
        function SetAutoPlay() {
            var elm = document.getElementById("autoPlayCB");
            var p = getMediaPlayer();
            p.AutoPlay = elm.checked;
        }
        function SetLoop() {
            var elm = document.getElementById("loopCB");
            var p = getMediaPlayer();
            p.Loop = elm.checked;
        }
        function SetDisplayMode() {
            var elm = document.getElementById("DisplayModeSelect");
            var p = getMediaPlayer();
            p.DisplayMode = elm.value;
        }

        //Sets back the time of the media being played by 5000 milliseconds (5 seconds).
        function BackFive() {
            var p = getMediaPlayer();
            var pos = p.PositionMilliseconds;
            pos -= 5000; /*5 seconds*/
            p.PositionMilliseconds = pos;
            ShowPosition();
            ShowPositionMilliseconds();
        }
        //Plays media set in the MediaURLField.
        function Play() {
            var p = getMediaPlayer();
            p.Play();
        }
        //Pauses media.
        function Pause() {
            var p = getMediaPlayer();
            p.Pause();
        }
        //Stops media.
        function Stop() {
            var p = getMediaPlayer();
            p.Stop();
        }
        //Sets forward the time of the media being played by 5000 milliseconds (5 seconds).
        function ForwardFive() {
            var p = getMediaPlayer();
            var pos = p.PositionMilliseconds;
            pos += 5000; /*5 seconds*/
            p.PositionMilliseconds = pos;
            ShowPosition();
            ShowPositionMilliseconds();
        }
        //Sets back the time of the media being played by 5000 milliseconds (5 seconds).
        function ShowEmbedText() {
            var p = getMediaPlayer();
            var elm = document.getElementById("EmbedHost");
            if (elm.innerText != undefined) {
                elm.innerText = p.EmbedText;
            }
            else {
                elm.textContent = p.EmbedText;
            }
        }
        //Shows the total duration (in minute:second format) of the selected media.
        function ShowDuration() {
            var p = getMediaPlayer();
            var elm = document.getElementById("DurationHost");
            if (elm.innerText != undefined) {
                elm.innerText = p.Duration;
            }
            else {
                elm.textContent = p.Duration;
            }
        }
        //Shows the total duration (in milliseconds) of the selected media.
        function ShowDurationMilliseconds() {
            var p = getMediaPlayer();
            var elm = document.getElementById("DurationMillisecondsHost");
            if (elm.innerText != undefined) {
                elm.innerText = p.DurationMilliseconds;
            }
            else {
                elm.textContent = p.DurationMilliseconds;
            }
        }
        //By default, gets the position in minutes and seconds of the selected media based on internal text; otherwise, gets the position in minutes and seconds of the selected media based on metadata.
        function ShowPosition() {
            var p = getMediaPlayer();
            var elm = document.getElementById("PositionHost");
            if (elm.innerText != undefined) {
                elm.innerText = p.Position;
            }
            else {
                elm.textContent = p.Position;
            }
        }
        // By default, gets the position in milliseconds of the selected media based on internal text; otherwise, gets the position in milliseconds of the selected media based on metadata.
        function ShowPositionMilliseconds() {
            var p = getMediaPlayer();
            var elm = document.getElementById("PositionMillisecondsHost");
            if (elm.innerText != undefined) {
                elm.innerText = p.PositionMilliseconds;
            }
            else {
                elm.textContent = p.PositionMilliseconds;
            }
        }
    </script>
    <style>
    .propertyVal
    {
      background-color: cornsilk;
      font-weight: bolder;
    }
    </style>
  <head>
<!--[if gte mso 9]><![endif]-->
</head>
<body style="{font: 10pt, sans-serif;}">
    <div>
      <div>
      //Sets test controls with user-specified values.
      <form>
        <input type="text" id="MediaURLField"> <a href="javascript:SetMediaSource();">Set Media Source</a><br>
        <input type="text" id="PreviewURLField"> <a href="javascript:SetPreviewImageSource();">Set Preview Image Source</a><br>
        <input type="text" id="TitleField"> <a href="javascript:SetMediaTitle();">Set Media Title</a><br>
        <input type="text" id="TemplateURLField"> <a href="javascript:SetTemplateSource();">Set Template Source</a><br>
        <input type="checkbox" id="autoPlayCB"> <a href="javascript:SetAutoPlay();">Set Auto Play</a><br>
        <input type="checkbox" id="loopCB"> <a href="javascript:SetLoop();">Set Loop</a><br>
        <select id="DisplayModeSelect">
          <option>Overlay</option>
          <option selected="true">Inline</option>
          <option>Fullscreen</option>
        </select><a href="javascript:SetDisplayMode();">Set Display Mode</a><br><br>
        <a href="javascript:Play();">Play</a>
        <a href="javascript:Pause();">Pause</a>
        <a href="javascript:Stop();">Stop</a>
        <a href="javascript:BackFive();">Back 5</a>
        <a href="javascript:ForwardFive();">Forward 5</a><br><br>
        <a href="javascript:ShowEmbedText();">Show EmbedText</a> Embed Text:<span id="EmbedHost" class="propertyVal"></span><br>
        <a href="javascript:ShowDuration();">Show Duration</a> Duration:<span id="DurationHost" class="propertyVal"></span><br>
        <a href="javascript:ShowPosition();">Show Position</a> Position:<span id="PositionHost" class="propertyVal"></span><br>
        <a href="javascript:ShowDurationMilliseconds();">Show DurationMilliseconds</a> DurationMilliseconds:<span id="DurationMillisecondsHost" class="propertyVal"></span><br>
        <a href="javascript:ShowPositionMilliseconds();">Show PositionMilliseconds</a> PositionMilliseconds:<span id="PositionMillisecondsHost" class="propertyVal"></span><br>
      </form>
      </div>
      <div id="MediaPlayerHost">
        <script>
            {
                init();
                var MediaURL = document.getElementById("MediaURLField").value;
                var PreviewURL = document.getElementById("PreviewURLField").value;
                var MediaTitle = document.getElementById("TitleField").value;
                var AutoPlay = document.getElementById("autoPlayCB").checked;
                var Loop = document.getElementById("loopCB").checked;
                mediaPlayer.createMediaPlayer(
                  document.getElementById('MediaPlayerHost'),
                  'MediaPlayerHost',
                  '500px', '333px',
                  {
                      displayMode: 'Inline',
                      mediaTitle: '文档视频 show',
                      mediaSource: 'http://va.neu.edu.cn/CollegeComputer/Networks/DocLib/互联网学习/互联网学习.mp4',
                      previewImageSource: PreviewURL,
                      autoPlay: AutoPlay,
                      loop: Loop,
                      mediaFileExtensions: 'wmv;wma;avi;mpg;mp3;',
                      silverlightMediaExtensions: 'wmv;wma;mp3;'
                  });
            }
        </script>
      </div>
    </div>
  </body>
</html>
