using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace YetAnotherTest
{
    public class Map: IMap
    {
        List<Entity> _entities = new List<Entity>();
        public List<MapRow> Rows = new List<MapRow>();
        private int MapWidth = 50;
        private int MapHeight = 50;
        int TimerTickCount = 0;
        public float Height
        {
            get
            {
                return MapHeight * Tile.TileHeight;
            }
        }

        public float Width
        {
            get
            {
                return MapWidth * Tile.TileWidth;
            }
        }

        public IEnumerable<Entity> Entities
        {
            get { return _entities; }
        }

        public Map()
        {


        }

        public void AddEntity(Entity entity)
        {
            if (!_entities.Contains(entity))
            {
                _entities.Add(entity);
                entity.Moved += Entity_Moved;
            }
        }

        public void RemoveEntity(Entity entity)
        {
            if (_entities.Contains(entity))
            {
                entity.Moved -= Entity_Moved;
                _entities.Remove(entity);
            }
        }

        private void Entity_Moved(Entity entity)
        {
            Vector2 moveVector = entity.Position - entity.PreviousPosition;
            int x = (int)(entity.Position.X + Math.Ceiling(moveVector.X));
            int y = (int)(entity.Position.Y + Math.Ceiling(moveVector.Y));
            if (isObstacle(Rows[y].Columns[x].tile))
                entity.Position = entity.PreviousPosition;
        }

        public bool isObstacle(int tileId)
        {
            switch(tileId)
            {
                case 0:
                case 1:
                    return true;
                default: return false;
            }
        }

        public Map(int Width, int Height)
        {
            MapWidth = Width;
            MapHeight = Height;

            for (int y = 0; y < MapHeight; y++)
            {
                MapRow thisRow = new MapRow();
                for (int x = 0; x < MapWidth; x++)
                {
                    thisRow.Columns.Add(new MapCell(0));
                }
                Rows.Add(thisRow);
            }

            System.Timers.Timer gc = new System.Timers.Timer(1000);
            gc.Elapsed += Gc_Elapsed;
            gc.Start();
        }

        private void Gc_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void SaveMap()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Tile map|*.tmap";
            sfd.Title = "Save an Tile Map File";
            sfd.ShowDialog();
            if (sfd.FileName != "")
            {
                try
                {
                    using (TextWriter stream = new StreamWriter(sfd.FileName))
                    {
                        XmlSerializer xml = new XmlSerializer(typeof(Map));
                        xml.Serialize(stream, this);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        public static Map Load(string path)
        {
            try
            {
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(Map));
                    return (Map)xml.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            throw new FileNotFoundException();
        }

        public void Update(GameTime time)
        {
            foreach (var entity in Entities)
            {
               entity.Update(time);
            }
        }
    }
}
