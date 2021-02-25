using System;
using System.Diagnostics;
using System.Windows.Forms;

public class CountDownTimer : IDisposable
{
    public Stopwatch stopWatch = new Stopwatch();

    public Action TimeChanged;
    public Action CountDownFinished;

    public bool IsRunning => timer.Enabled;

    public int StepMs
    {
        get => timer.Interval;
        set => timer.Interval = value;
    }

    private Timer timer = new Timer();

    private TimeSpan _max = TimeSpan.FromMilliseconds(30000);

    public TimeSpan TimeLeft => (_max.TotalMilliseconds - stopWatch.ElapsedMilliseconds) > 0 ? TimeSpan.FromMilliseconds(_max.TotalMilliseconds - stopWatch.ElapsedMilliseconds) : TimeSpan.FromMilliseconds(0);

    private bool mustStop => (_max.TotalMilliseconds - stopWatch.ElapsedMilliseconds) < 0;


    private void TimerTick(object sender, EventArgs e)
    {
        TimeChanged?.Invoke();

        if (mustStop)
        {
            CountDownFinished?.Invoke();
            stopWatch.Stop();
            timer.Enabled = false;
        }
    }
    /// <summary>
    /// Countdowntimer med minuter och sekunder
    /// </summary>
    /// <param name="min"></param>
    /// <param name="sec"></param>
    public CountDownTimer(int min, int sec)
    {
        SetTime(min, sec);
        Init();
    }
    /// <summary>
    /// Countdown med timespan
    /// </summary>
    /// <param name="ts"></param>
    public CountDownTimer(TimeSpan ts)
    {
        SetTime(ts);
        Init();
    }

    public CountDownTimer()
    {
        Init();
    }

    private void Init()
    {
        StepMs = 1000;
        timer.Tick += new EventHandler(TimerTick);
    }

    public void SetTime(TimeSpan ts)
    {
        _max = ts;
        TimeChanged?.Invoke();
    }

    public void SetTime(int min, int sec = 0) => SetTime(TimeSpan.FromSeconds(min * 60 + sec));

    public void Start()
    {
        timer.Start();
        stopWatch.Start();
    }

    public void Pause()
    {
        timer.Stop();
        stopWatch.Stop();
    }

    public void Stop()
    {
        Reset();
        Pause();
    }

    public void Reset()
    {
        stopWatch.Reset();
    }

    public void Restart()
    {
        stopWatch.Reset();
        timer.Start();
    }

    public void Dispose() => timer.Dispose();
}