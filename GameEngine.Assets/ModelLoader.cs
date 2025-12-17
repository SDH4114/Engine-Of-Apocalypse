// ============================================================================
// ЗАГРУЗЧИК МОДЕЛЕЙ (FBX, GLTF, OBJ)
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace GameEngine.Core
{
    // ========================================================================
    // ЗАГРУЗЧИК МОДЕЛЕЙ (FBX, GLTF, OBJ)
    // ========================================================================
    public class ModelLoader
    {
        public async Task<Mesh> LoadAsync(string path)
        {
            return await Task.Run(() => Load(path));
        }

        public Mesh Load(string path)
        {
            var extension = Path.GetExtension(path).ToLower();

            return extension switch
            {
                ".obj" => LoadOBJ(path),
                ".fbx" => LoadFBX(path),
                ".gltf" => LoadGLTF(path),
                ".glb" => LoadGLB(path),
                _ => throw new NotSupportedException($"Format {extension} not supported")
            };
        }

        private Mesh LoadOBJ(string path)
        {
            // Простой парсер OBJ
            var vertices = new List<Vertex>();
            var indices = new List<uint>();
            var positions = new List<Vector3>();
            var normals = new List<Vector3>();
            var texCoords = new List<Vector2>();

            foreach (var line in File.ReadLines(path))
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0) continue;

                switch (parts[0])
                {
                    case "v": // Позиция вершины
                        positions.Add(new Vector3(
                            float.Parse(parts[1]),
                            float.Parse(parts[2]),
                            float.Parse(parts[3])));
                        break;

                    case "vn": // Нормаль
                        normals.Add(new Vector3(
                            float.Parse(parts[1]),
                            float.Parse(parts[2]),
                            float.Parse(parts[3])));
                        break;

                    case "vt": // Текстурные координаты
                        texCoords.Add(new Vector2(
                            float.Parse(parts[1]),
                            float.Parse(parts[2])));
                        break;

                    case "f": // Грань
                        for (int i = 1; i <= 3; i++)
                        {
                            var vertexData = parts[i].Split('/');
                            var posIndex = int.Parse(vertexData[0]) - 1;
                            var texIndex = vertexData.Length > 1 && !string.IsNullOrEmpty(vertexData[1]) 
                                ? int.Parse(vertexData[1]) - 1 : 0;
                            var normIndex = vertexData.Length > 2 
                                ? int.Parse(vertexData[2]) - 1 : 0;

                            vertices.Add(new Vertex(
                                positions[posIndex],
                                normals.Count > 0 ? normals[normIndex] : Vector3.UnitY,
                                texCoords.Count > 0 ? texCoords[texIndex] : Vector2.Zero));
                            
                            indices.Add((uint)(vertices.Count - 1));
                        }
                        break;
                }
            }

            var mesh = new Mesh
            {
                Vertices = vertices.ToArray(),
                Indices = indices.ToArray()
            };
            mesh.Upload();
            return mesh;
        }

        private Mesh LoadFBX(string path)
        {
            // Используйте библиотеку Assimp.Net для загрузки FBX
            // var importer = new Assimp.AssimpContext();
            // var scene = importer.ImportFile(path, PostProcessSteps.Triangulate | PostProcessSteps.GenerateNormals);
            
            Console.WriteLine("FBX loading requires Assimp.Net library");
            return Mesh.CreateCube(); // Заглушка
        }

        private Mesh LoadGLTF(string path)
        {
            // Используйте библиотеку SharpGLTF для загрузки GLTF
            // var model = SharpGLTF.Schema2.ModelRoot.Load(path);
            
            Console.WriteLine("GLTF loading requires SharpGLTF library");
            return Mesh.CreateCube(); // Заглушка
        }

        private Mesh LoadGLB(string path)
        {
            return LoadGLTF(path); // GLB - это бинарная версия GLTF
        }
    }
}