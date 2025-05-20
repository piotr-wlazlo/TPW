//__________________________________________________________________________________________
//
//  Copyright 2024 Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and to get started
//  comment using the discussion panel at
//  https://github.com/mpostol/TP/discussions/182
//__________________________________________________________________________________________

using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TP.ConcurrentProgramming.Presentation.Model;
using TP.ConcurrentProgramming.Presentation.ViewModel.MVVMLight;
using ModelIBall = TP.ConcurrentProgramming.Presentation.Model.IBall;

namespace TP.ConcurrentProgramming.Presentation.ViewModel {
    public class MainWindowViewModel : ViewModelBase, IDisposable {
        #region ctor

        public MainWindowViewModel() : this(null) { }

        internal MainWindowViewModel(ModelAbstractApi modelLayerAPI) {
            ModelLayer = modelLayerAPI == null ? ModelAbstractApi.CreateModel() : modelLayerAPI;
            Observer = ModelLayer.Subscribe<ModelIBall>(x => Balls.Add(x));
            StartCommand = new RelayCommand(Start);
            StopCommand = new RelayCommand(Stop);
            AddBallCommand = new RelayCommand(AddBall, CanAddBall);
            RemoveBallCommand = new RelayCommand(RemoveBall, CanRemoveBall);
        }

        #endregion ctor

        #region public API

        public ObservableCollection<ModelIBall> Balls { get; } = new ObservableCollection<ModelIBall>();

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }
        public ICommand AddBallCommand { get; }
        public ICommand RemoveBallCommand { get; }

        public bool isRunning = false;
        public bool IsRunning {
            get => isRunning;

            set {
                if (isRunning != value) {
                    isRunning = value;
                    RaisePropertyChanged();
                    ((RelayCommand)AddBallCommand).RaiseCanExecuteChanged();
                    ((RelayCommand)RemoveBallCommand).RaiseCanExecuteChanged();
                }
            }
        }

        private int ballCount = 1;

        public int BallCount {
            get => ballCount;
            set {
                if (value < 1)
                    value = 1;
                else if (value > 100)
                    value = 100;

                if (ballCount != value) {
                    ballCount = value;
                    RaisePropertyChanged();

                    if (IsRunning) {
                        int count = Balls.Count;
                        if (value > count) {
                            for (int i = count; i < value; i++) {
                                ModelLayer.AddBall();
                            }
                        }
                        else if (value < count) {
                            for (int i = count - 1; i >= value; i--) {
                                ModelLayer.RemoveBall();
                                Balls.RemoveAt(i);
                            }
                        }
                        ((RelayCommand)RemoveBallCommand).RaiseCanExecuteChanged();
                    }
                }
            }
        }

        public void Start() {
            if (!IsRunning) {
                IsRunning = true;
                ModelLayer.Start(BallCount);
                ((RelayCommand)RemoveBallCommand).RaiseCanExecuteChanged();
            }
        }

        public void Stop() {
            if (IsRunning) {
                IsRunning = false;

                while (Balls.Count > 0) {
                    ModelLayer.RemoveBall();
                    Balls.RemoveAt(Balls.Count - 1);
                }
                BallCount = 0;
            }
        }

        public void AddBall() {
            if (IsRunning) {
                ModelLayer.AddBall();
                BallCount = Balls.Count;
            }
        }

        public bool CanAddBall() {
            return IsRunning && Balls.Count < 100;
        }

        public void RemoveBall() {
            if (IsRunning && Balls.Count > 1) {
                IsRunning = false;

                ModelLayer.RemoveBall();
                Balls.RemoveAt(Balls.Count - 1);
                BallCount = Balls.Count;

                IsRunning = true;

                ((RelayCommand)AddBallCommand).RaiseCanExecuteChanged();
                ((RelayCommand)RemoveBallCommand).RaiseCanExecuteChanged();
            }
        }

        public bool CanRemoveBall() {
            return IsRunning && Balls.Count > 1;
        }

        #endregion public API

        #region IDisposable

        protected virtual void Dispose(bool disposing) {
            if (!Disposed) {
                if (disposing) {
                    Balls.Clear();
                    Observer.Dispose();
                    ModelLayer.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                Disposed = true;
            }
        }

        public void Dispose() {
            if (Disposed)
                throw new ObjectDisposedException(nameof(MainWindowViewModel));
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable

        #region private

        private IDisposable Observer = null;
        private ModelAbstractApi ModelLayer;
        private bool Disposed = false;

        #endregion private
    }
}