// Copyright (c) 2023, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.ComponentModel;

namespace Moryx.Workplans
{
    /// <summary>
    /// Default implementation of IWorkplan
    /// </summary>
    [DataContract]
    public class Workplan : IWorkplan, IPersistentObject
    {
        /// <summary>
        /// Create a new workplan instance
        /// </summary>
        public Workplan() : this(new List<IConnector>(), new List<IWorkplanStep>())
        {
        }

        /// <summary>
        /// Private constructor used for new and restored workplans
        /// </summary>
        private Workplan(List<IConnector> connectors, List<IWorkplanStep> steps)
        {
            _connectors = connectors;
            _steps = steps;
        }

        /// <see cref="IWorkplan"/>
        public long Id { get; set; }

        ///<see cref="IWorkplan"/>
        public string Name { get; set; }

        ///<see cref="IWorkplan"/>
        public int Version { get; set; }

        ///<see cref="IWorkplan"/>
        public WorkplanState State { get; set; }

        /// <summary>
        /// Current biggest id in the workplan
        /// </summary>
        public int MaxElementId { get; set; }

        /// <summary>
        /// Editable list of connectors
        /// </summary>
        [DataMember]
        private List<IConnector> _connectors;

        /// <see cref="IWorkplan"/>
        public IEnumerable<IConnector> Connectors => _connectors;

        /// <summary>
        /// Editable list of steps
        /// </summary>
        [DataMember]
        private List<IWorkplanStep> _steps;

        /// <see cref="IWorkplan"/>
        public IEnumerable<IWorkplanStep> Steps => _steps;

        /// <summary>
        /// Add a range of connectors to the workplan
        /// </summary>
        public void Add(params IWorkplanNode[] nodes)
        {
            foreach (var node in nodes)
            {
                node.Id = ++MaxElementId;
                if (node is IConnector)
                    _connectors.Add((IConnector)node);
                else
                    _steps.Add((IWorkplanStep)node);
            }
        }
        /// <summary>
        /// Removes a node from the workplan
        /// </summary>
        public bool Remove(IWorkplanNode node)
        {
            return node is IConnector ? _connectors.Remove((IConnector)node) : _steps.Remove((IWorkplanStep)node);
        }

        /// <summary>
        /// Restore a workplan with a list of connectors and steps
        /// </summary>
        public static Workplan Restore(List<IConnector> connectors, List<IWorkplanStep> steps)
        {
            return new Workplan(connectors, steps);
        }

        public override bool Equals(object obj)
        {
            if(!(obj is Workplan))
            {
                return false;
            }
            Workplan newPlan = (Workplan)obj;

        
            //start, end und failed werden festgelegt
            var start = this.Connectors.First(x => x.Name.Equals("Start"));
            var end = this.Connectors.First(x => x.Name.Equals("End"));
            var failed = this.Connectors.First(x => x.Name.Equals("Failed"));
            var next = this.Steps.First(x => x.Inputs.Any(y => y.Equals(start)));

            var newStart = newPlan.Connectors.First(x => x.Name.Equals("Start"));
            var newEnd = newPlan.Connectors.First(x => x.Name.Equals("End"));
            var newFailed = newPlan.Connectors.First(x => x.Name.Equals("Failed"));
            var newNext = newPlan.Steps.First(x => x.Inputs.Any(y => y.Equals(newStart)));

            //Listen zur Notiz werden erstellt 
            List<IWorkplanStep> note = new List<IWorkplanStep>();
            List<IWorkplanStep> newNote = new List<IWorkplanStep>();

            //Listen für die bereits geprüften Elemente
            List<IWorkplanStep> check = new List<IWorkplanStep>();
            List<IWorkplanStep> newCheck = new List<IWorkplanStep>();


            if (next.Outputs.Length == newNext.Outputs.Length) //Bedingung: beide Listen gleich viele Connectoren
            {
                //erstes Element nach Start wird in der Note-Liste gespeichert 
                note.Add(next);
                newNote.Add(newNext); 
            }
            else
            {
                return false;
            }

            while (note.Count != 0 && newNote.Count != 0) //solange die Liste gefüllt ist 
            {
                //Zählvariable für Outputs
                int a = 0;

                while (a < next.Outputs.Length)
                {
                    //festelegen des Connectors an dem jeweiligen output
                    var c = next.Outputs[a];
                    var newC = newNext.Outputs[a];


                    if (c != end && newC != newEnd) //Abfrage, ob "End" am Output ist 
                    {
                        if (c != failed && newC != newFailed) //Abfrage, ob "Failed" am Output ist
                        {
                            //speichern des nächsten Elements an diesem Connector
                            var t = this.Steps.FirstOrDefault(x => x.Inputs.Any(y => y.Equals(c)));
                            var newT = newPlan.Steps.FirstOrDefault(x => x.Inputs.Any(y => y.Equals(newC)));

                            if (!(check.Contains(t) && newCheck.Contains(newT))) //Überprüfung, ob das Element bereits geprüft wurde
                            {

                                //das Element an diesem Output wird zur Liste hinzugefügt 
                                note.Add(t);
                                newNote.Add(newT);


                            }
                        }
                        else 
                        {
                            if (c.Classification != newC.Classification)
                            {
                                return false;
                            }
                        }
                    }

                    else 

                   {
                        if (c.Classification != newC.Classification)
                        {
                            return false;
                        }
                    }
                    a++;
                }
                //Vergleich der Steps
                if (next.GetType() == newNext.GetType())
                {
                    //das abgearbeitete Element wird aus der Liste entfernt 
                    note.RemoveAll(x => x.Equals(next));
                    newNote.RemoveAll(x => x.Equals(newNext));

                    //das abgearbeitete Element wir in die 'check'-Liste hinzugefügt
                    check.Add(next);
                    newCheck.Add(newNext);

                    if (note.Count != 0 && newNote.Count != 0)
                    {
                        //"next" wird mit dem nächsten Element der Liste überschrieben 
                        next = note[0];
                        newNext = newNote[0];
                    }
                }
                else
                {
                    return false;
                }

            }
            //beide Pläne sind identisch
            return true;
            

        }
    }
}
