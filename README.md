# AppFiguresRanks
Xamarin Android and iOS app which shows your (real) mobile apps' ranks via AppFigures account.

# How to make it work?
First of all, you need an account at https://appfigures.com/ - these guys provide API which is free for apps that you own. I mean, you should be an owner in terms of AppFigures - not the one who was granted an access.

Not quite sure if the account has to be paid. Sorry for that - I took it from our marketing department, so ours _is_ paid.

Second, you need to fill the `Credentials.cs` like that:

    using System;

    namespace Rankings_Common
    {
        public static class Credentials
        {
            public static readonly String Username = "YOUR_LOGIN_TO_APPFIGURES";
            public static readonly String Password = "YOUR_PASSWORD";
            public static readonly String ClientKey = "API_KEY";
        }
    }

Instructions on API key are here: https://appfigures.com/developers/keys

When generating it, add `Account info`, `Product meta data` and `Public data` permissions. I ended up with this combination - maybe something is redundant, but anyway.

# Restrictions

This app was made via starter (i.e. free) edition of Xamarin. That's why I didn't use Xamarin.Forms and even Json.Net.

But on the bright side it can be built without payments. And it was an interesting challenge, by the way.

# What to do if you don't like it

Nobody is perfect. This was made for fun and can't be treated as a masterpiece. iOS code here probably sucks more because I'm an Android developer and it was my very first attempt to make it work.

So, if you don't like something here, just make it better. It's open source after all.
