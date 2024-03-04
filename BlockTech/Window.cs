using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace BlockTech;

public class Game : GameWindow
{
    private float[] _vertices = {
           // position      //color
        -0.9f,-0.9f, 0.0f, 1.0f, 0.0f, 0.0f,
         0.9f,-0.9f, 0.0f, 0.0f, 1.0f, 0.0f,
         0.9f, 0.9f, 0.0f, 0.0f, 0.0f, 1.0f,
        -0.9f, 0.9f, 0.0f, 0.0f, 0.0f, 0.0f,
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

        this._vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, this._vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, this._vertices.Length * sizeof(float), this._vertices, BufferUsageHint.StaticDraw);
        
        this._vao = GL.GenVertexArray();        

        GL.BindVertexArray(this._vao);

        // VertexAttribPointer(index, size, type, normalized, stride, offset);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexArrayAttrib(this._vao, 0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexArrayAttrib(this._vao, 1);

        // unbind vbo
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        this._ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, this._ebo);

        GL.BufferData(BufferTarget.ElementArrayBuffer, this._indices.Length * sizeof(uint), this._indices, BufferUsageHint.StaticDraw);

        // unbind ebo
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        
        //unbind vao
        GL.BindVertexArray(0);
        
        
        this._shader = new Shader("default.vert", "default.frag");
        this._shader.Use();

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
        GL.ClearColor(0.4f, 0.6f, 1.0f, 1.0f);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        GL.BindVertexArray(this._vao);
        GL.DrawElements(PrimitiveType.Triangles, this._indices.Length, DrawElementsType.UnsignedInt, 0);


        Context.SwapBuffers();

        base.OnRenderFrame(args);
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
}