# PlexPowerSaver
_Power off an idle Plex server._

### The Problem
Rather than leaving my main machine on as a server all the time I enabled S5 WOL (WakeOnLAN) to power it on remotely on demand. However, I wanted a way to make sure the system was never left on longer than it needed to be, the electric bill was getting out of hand.

### The Solution
I decided it should shutdown itself when some criteria were met:

### The Criteria
* No user input for set ammount of time (System Idle), default 30 mins.
* No blacklisted programs are running, default currently just the Plex transcoder (PlexNewTranscoder.exe).
* No Plex streams are currently playing.
* No Plex stream finished in the last 10 minutes.

### The Result
If the computer is not in active use, nothing is playing or being prepared for play, or just incase its not likely that we are just between two videos looking for the next thing to watch... turn it off.

...and hopefully a lower power bill.
