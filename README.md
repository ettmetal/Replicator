# Replicator 🌌

Whooshy GameObject pooling / reuse for Unity.

I've been working on this system on and off for a little while. I decided it should finally see the light of day 🌄.

✨ __Features:__

- Object pooling
- Pools are assets in your project, making them easy to configure
- Easy to replace existing code

🚧 __Replicator is still under construction.__ *There is __no stable version__ yet* 🚧

__Current Version:__ [0.1-alpha](../../releases/tag/v0.1.1-alpha)

## Getting Started

Replicator makes it simple to rework existing code to use object pooling. Set up a pool, swap `Instantiate` calls to `Spawn` and `Destroy` calls to `Recycle`.

First, create an Object Pool in your project (right click -> Create -> Object Pool, grouped next to Prefab Variant).

Configure the pool with the prefab to use etc. (Have no fear - until the manual apears here, the fields have tooltips to guide you)

```csharp
using Replicator;

public class MyScript : MonoBehaviour {
    public GameObject prefab;

    void Update() {
        GameObject clone1 = Spawn(prefab, Vector3.zero, Quaternion.identity);
        GameObject clone2 = prefab.Spawn(Vector3.zero, Quaternion.identity);
        // ... Some time later:
        Recycle(clone1);
        clone2.Recycle();
    }
}
```

With a flexible API, there are several ways to Spawn / Recycle GameObjects in whichever way makes sense for your project. If you prefer to interface with the pool directly, you can.

Full details can be found at the [API Wiki](../../wiki/API).

## Installing

Head over to [Releases](../../releases) and download the most recent version appropriate for your Unity version. Import the package into your Unity project.

If you'd rather work on Replicator itself, it's much easier to just clone the repository, which is a complete Unity project.

## Contributing

Found a bug 🐛?
Have some feedback 💭?
Want to add a feature to the wishlist ✨?

Here are some ways you can help make Replicator better:

- Open an [Issue](../../issues) 🐛💭✨
- Contact [@ettmetal] on Twitter 💭✨
- Send a [PR](../../pulls) 🐛✨

[@ettmetal]: https://twitter.com/ettmetal

## License

Copyright © 2019 [Joe Osborne](https://gihub.com/ettmetal/).

Replicator is released under the MIT license. Refer to [LICENSE.md](LICENSE.md) in this repository for more information.
