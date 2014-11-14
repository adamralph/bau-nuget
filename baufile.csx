var bau = Require<Bau>();

bau

.Task("poop").Do(() => {Console.WriteLine("poop...");})

.Run();