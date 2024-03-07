using StbImageSharp;
using System.IO;
using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Common.Input;


namespace BlockTech;

public class Game : GameWindow
{
    private float[] _vertices = {
        //    position       tex pos
         0.5f, 0.5f, 0.0f, 1.0f, 1.0f, // top right
         0.5f,-0.5f, 0.0f, 1.0f, 0.0f, // bottom right
        -0.5f,-0.5f, 0.0f, 0.0f, 0.0f, // bottom left
        -0.5f, 0.5f, 0.0f, 0.0f, 1.0f, // top left
    };

    private uint[] _indices =
    {
        0, 1, 3,
        1, 2, 3

    };

    private int _vao;
    private int _vbo;
    private int _ebo;
    private Shader? _shader;
    private Texture? _texture;


    public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
     : base(gameWindowSettings, nativeWindowSettings)
    {
    }
    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
    }
    protected override void OnLoad()
    {
        base.OnLoad();

        this.MinimumSize = new Vector2i(100, 100);
        
        this.CreateIcon("assets/textures/grass_block_side.png");

        this._vao = GL.GenVertexArray();        
        GL.BindVertexArray(this._vao);



        this._vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, this._vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, this._vertices.Length * sizeof(float), this._vertices, BufferUsageHint.StaticDraw);

        this._ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, this._ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, this._indices.Length * sizeof(uint), this._indices, BufferUsageHint.StaticDraw);

        this._shader = new Shader("default.vert", "default.frag");
        this._shader.Use();


        int vertexLocation = _shader.GetAttribLocation("aPosition");
        GL.EnableVertexAttribArray(vertexLocation);
        GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

        int texLocation = _shader.GetAttribLocation("aTexCoord");
        GL.EnableVertexAttribArray(texLocation);
        GL.VertexAttribPointer(texLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

        

        
        //unbind
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
        
        this._texture = new Texture("grass_block_side.png");
        this._texture.Use(TextureUnit.Texture0); 

        GL.GetInteger(GetPName.MaxVertexAttribs, out int maxAttributeCount);
        Logger.Debug($"Maximum number of vertex attributes supported: {maxAttributeCount}");

    }
    protected override void OnUnload()
    {
        base.OnUnload();

        GL.DeleteVertexArray(this._vao);
        GL.DeleteBuffer(this._vbo);
        GL.DeleteBuffer(this._ebo);
    }
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.ClearColor(0.4f, 0.6f, 1.0f, 1.0f);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        GL.BindVertexArray(this._vao);

        this._texture?.Use(TextureUnit.Texture0);
        this._shader?.Use();

        GL.DrawElements(PrimitiveType.Triangles, this._indices.Length, DrawElementsType.UnsignedInt, 0);

        Context.SwapBuffers();
    }
    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        KeyboardState input = KeyboardState;

        if (input.IsKeyPressed(Keys.F11))
        {
            if (this.WindowState == WindowState.Fullscreen)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Fullscreen;
            }
        }
    }
    private void CreateIcon(string path) {        
        using (Stream stream = File.OpenRead(path))
        {
            ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

            this.Icon = new WindowIcon(new OpenTK.Windowing.Common.Input.Image(image.Width, image.Height, image.Data));
        }

    }
}