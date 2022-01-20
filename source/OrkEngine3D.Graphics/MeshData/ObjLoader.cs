﻿using OrkEngine3D.Graphics.TK;
using OrkEngine3D.Mathematics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrkEngine3D.Graphics.MeshData
{
    public static class ObjLoader
    {
        public static MeshInformation LoadObjData(string text, out string mtl)
        {
            string[] lines = text.Split('\n');
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<uint> triangles = new List<uint>();


            List<uint> tvertices = new List<uint>();
            List<uint> tuvs = new List<uint>();
            List<uint> tnormals = new List<uint>();

            mtl = "";


            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();

                if (line.StartsWith("v "))
                {
                    var c = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (c.Length < 4)
                        throw new Exception("Invalid OBJ file!");
                    float x = float.Parse(c[1], CultureInfo.InvariantCulture);
                    float y = float.Parse(c[2], CultureInfo.InvariantCulture);
                    float z = float.Parse(c[3], CultureInfo.InvariantCulture);
                    vertices.Add(new Vector3(x, y, z));
                }
                if (line.StartsWith("vt "))
                {
                    var c = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (c.Length < 3)
                        throw new Exception("Invalid OBJ file!");
                    float x = float.Parse(c[1], CultureInfo.InvariantCulture);
                    float y = float.Parse(c[2], CultureInfo.InvariantCulture);
                    uvs.Add(new Vector2(x, y));
                }
                if (line.StartsWith("vn "))
                {
                    var c = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (c.Length < 4)
                        throw new Exception("Invalid OBJ file!");
                    float x = float.Parse(c[1], CultureInfo.InvariantCulture);
                    float y = float.Parse(c[2], CultureInfo.InvariantCulture);
                    float z = float.Parse(c[3], CultureInfo.InvariantCulture);
                    normals.Add(new Vector3(x, y, z));
                }
                if (line.StartsWith("f "))
                {
                    var c = line.Split(' ');
                    if (c.Length < 4)
                        throw new Exception("Invalid OBJ file!");

                    for (int s = 1; s < 4; s++)
                    {
                        string set = c[s];
                        string[] t = set.Split('/');

                        if (t.Length < 3)
                            throw new Exception("Invalid OBJ file!");

                        uint v = uint.Parse(t[0]);
                        uint u = uint.Parse(t[1]);
                        uint n = uint.Parse(t[2]);

                        tvertices.Add(v - 1);
                        tuvs.Add(u - 1);
                        tnormals.Add(n - 1);

                    }
                }
                if(line.StartsWith("mtllib "))
                {
                    mtl = line.Substring("mtllib ".Length).Trim();
                }
            }

            List<Vector3> fvertices = new List<Vector3>();
            List<Vector2> fuvs = new List<Vector2>();
            List<Vector3> fnormals = new List<Vector3>();
            List<uint> ftriangles = new List<uint>();

            for (int i = 0; i < tvertices.Count; i++)
            {
                uint vertexIndex = tvertices[i];
                uint normalIndex = tnormals[i];
                uint uvIndex = tuvs[i];
                fvertices.Add(vertices[(int)vertexIndex]);
                fnormals.Add(normals[(int)normalIndex]);
                fuvs.Add(uvs[(int)uvIndex]);
                ftriangles.Add((uint)i);
            }

            return new MeshInformation(fvertices.ToArray(), new Color4[0], fuvs.ToArray(), fnormals.ToArray(), ftriangles.ToArray());
        }

        public static Material LoadMTLFromFile(string content)
        {
            Material material = new Material();
            string[] lines = content.Split('\n', StringSplitOptions.TrimEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if(line.StartsWith("Ka "))
                {
                    var c = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (c.Length < 4)
                        throw new Exception("Invalid MTL file!");
                    float r = float.Parse(c[1], CultureInfo.InvariantCulture);
                    float g = float.Parse(c[2], CultureInfo.InvariantCulture);
                    float b = float.Parse(c[3], CultureInfo.InvariantCulture);
                    material.ambient = new Color3(r, g, b);
                }
                if (line.StartsWith("Kd "))
                {
                    var c = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (c.Length < 4)
                        throw new Exception("Invalid MTL file!");
                    float r = float.Parse(c[1], CultureInfo.InvariantCulture);
                    float g = float.Parse(c[2], CultureInfo.InvariantCulture);
                    float b = float.Parse(c[3], CultureInfo.InvariantCulture);
                    material.diffuse = new Color3(r, g, b);
                }
                if (line.StartsWith("Ks "))
                {
                    var c = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (c.Length < 4)
                        throw new Exception("Invalid MTL file!");
                    float r = float.Parse(c[1], CultureInfo.InvariantCulture);
                    float g = float.Parse(c[2], CultureInfo.InvariantCulture);
                    float b = float.Parse(c[3], CultureInfo.InvariantCulture);
                    material.specular = new Color3(r, g, b);
                }
                if (line.StartsWith("Ns "))
                {
                    var c = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (c.Length < 2)
                        throw new Exception("Invalid MTL file!");
                    material.shininess = float.Parse(c[1], CultureInfo.InvariantCulture);
                }
            }

            return material;
        }
    }
}
